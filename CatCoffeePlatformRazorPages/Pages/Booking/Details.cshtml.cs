using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;
using DAO.Context;
using DTO.BookingDTO;
using CatCoffeePlatformRazorPages.Common;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using BusinessObject.Enums;
using System.Net.Http.Headers;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace CatCoffeePlatformRazorPages.Pages.Booking
{
    public class DetailsModel : PageModel
    {
        private readonly ApiHelper _bookingApi;
        private readonly StripeSetting _stripeSetting;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient client = null!;
        private string BookingApiUrl = "";

        public DetailsModel(IOptions<StripeSetting> stripeSetting, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            _bookingApi = new ApiHelper("bookings");
            _stripeSetting = stripeSetting.Value;
            _httpContextAccessor = httpContextAccessor;
            BookingApiUrl = "https://localhost:7039/api/Bookings";
        }

        public BookingResponseDTO Booking { get; set; } = default!;
        public bool IsSuccess { get; set; } = false;

        [BindProperty]
        public int? BookingId { get; set; }
        [BindProperty]
        public decimal TotalRentalPrice { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if(id is null)
            {
                ViewData["warning"] = "Booking not found";
                return Page();
            }
            var apiResponse = await _bookingApi.GetAsync<ResponseBody<BookingResponseDTO>>(id.ToString()!);
            if(apiResponse?.Result is null)
            {
                ViewData["warning"] = "Booking not found";
                return Page();
            }
            Booking = apiResponse.Result;
            return Page();
        }

        public async Task<IActionResult> OnPostPayOrderAsync()
        {
            var apiResponse = await _bookingApi.GetAsync<ResponseBody<BookingResponseDTO>>(BookingId.ToString() ?? "0");
            var existedBooking = apiResponse?.Result;
            if (existedBooking is null)
            {
                ViewData["warning"] = "Checkout is not complete";
                ViewData["errors"] = new string[1]
                {
                    "Booking information not found"
                };
                return Page();
            }

            var currency = "VND";
            var successUrl = $"https://localhost:7031/Booking/StripeSuccess?BookingId={existedBooking.BookingId}";
            var cancelUrl = $"https://localhost:7031/Booking/StripeCancel?BookingId={existedBooking.BookingId}";
            StripeConfiguration.ApiKey = _stripeSetting.SecretKey;
            var options2 = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions> {
                    new SessionLineItemOptions {
                        PriceData = new SessionLineItemPriceDataOptions {
                            Currency = currency,
                            UnitAmount = Convert.ToInt32(TotalRentalPrice), // Amount in the smallest currency unit (e.g., cents)
                            ProductData = new SessionLineItemPriceDataProductDataOptions {
                                Name = "Product Name",
                                Description = "Product Description"
                            }
                        },
                    Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl
            };

            var service = new SessionService();
            var session = service.Create(options2);
            return Redirect(session.Url);
        }

        public async Task<IActionResult> OnPostCancelAsync()
        {
            var apiResponse = await _bookingApi.GetAsync<ResponseBody<BookingResponseDTO>>(BookingId.ToString() ?? "0");
            var existedBooking = apiResponse?.Result;
            if (existedBooking is null)
            {
                ViewData["warning"] = "Cannot cancel";
                ViewData["errors"] = new string[1]
                {
                    "Booking information not found"
                };
                return Page();
            }

            var token = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "token")?.Value;
            if(token is null)
            {
                ViewData["warning"] = "Cannot cancel";
                ViewData["errors"] = new string[1]
                {
                    "Please login first"
                };
                return Page();
            }

            if (existedBooking.PaymentStatus == (int)PaymentStatus.Paid)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string strData = JsonSerializer.Serialize(new
                {
                    Status = (int)BookingStatus.Cancel,
                });
                var contentData = new StringContent(strData, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync($"{BookingApiUrl}/{BookingId}", contentData);
                string responseMessage = await response.Content.ReadAsStringAsync();
                var status = response.StatusCode.ToString();

                var responseObject = JObject.Parse(responseMessage);

                if (!response.IsSuccessStatusCode)
                {
                    var errors = responseObject?["errors"]?.ToObject<List<string>>() ?? new List<string>();
                    errors.Add("Cancel failed");
                    ViewData["warning"] = "Cannot cancel";
                    ViewData["errors"] = errors;
                    return Page();
                }

                TempData["Information"] = "Cancel Successfully";
            }
            else
            {
                _bookingApi.SetAuthorizationHeader(token);
                var deleteResposne = await _bookingApi.DeleteAsync(BookingId.ToString() ?? "0");
                if (!deleteResposne)
                {
                    ViewData["warning"] = "Cannot cancel";
                    ViewData["errors"] = new string[1]
                    {
                        "Cancel failed"
                    };
                    return Page();
                }
                TempData["Information"] = "Cancel Successfully";
            }

            return RedirectToPage("Index");
        }
    }
}
