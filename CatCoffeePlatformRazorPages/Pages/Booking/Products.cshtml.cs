using CatCoffeePlatformRazorPages.Common;
using DTO.AreaDTO;
using DTO.BookingDTO;
using DTO.CoffeeShopDTO;
using DTO.Common;
using DTO.TimeFrameDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using Stripe.Checkout;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace CatCoffeePlatformRazorPages.Pages.Booking
{
    public class ProductsModel : PageModel
    {
        private readonly HttpClient client = null!;
        private readonly ApiHelper _apiProduct;
        private readonly ApiHelper _apiTimeFrame;
        private readonly ApiHelper _apiBooking;
        private string ProductUrl = "";
        private IHttpContextAccessor httpContextAccessor;
        private readonly StripeSetting _stripeSetting;

        public ProductsModel(IHttpContextAccessor httpContextAccessor, IOptions<StripeSetting> stripeSetting)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            _apiProduct = new ApiHelper("product");
            _apiTimeFrame = new ApiHelper(ApiResources.TimeFrames);
            _apiBooking = new ApiHelper(ApiResources.Bookings);
            ProductUrl = "https://localhost:7039/api/Product";
            this.httpContextAccessor = httpContextAccessor;
            _stripeSetting = stripeSetting.Value;
        }

        public CoffeeShopResponseDTO? CoffeeShop { get; set; }
        public AreaDto? Area { get; set; }
        public TimeFrameDto? TimeFrame { get; set; }

        [BindProperty]
        public string? CoffeeShopJson { get; set; }
        [BindProperty]
        public string? AreaJson { get; set; }
        [BindProperty]
        public int? TimeFrameId { get; set; }
        [BindProperty]
        [ModelBinder(BinderType = typeof(DateOnlyModelBinder))]
        public DateOnly? BookedDate { get; set; }
        [BindProperty]
        public int? BookedSlots { get; set; } = 0;
        [BindProperty]
        public decimal TotalRentalPrice { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; } = "";
        [BindProperty]
        public int? BookingId { get; set; }

        public IList<BusinessObject.Model.Product> Products { get; set; } = new List<BusinessObject.Model.Product>();


        public async Task<IActionResult> OnPostAsync()
        {
            var isAuthenticated = httpContextAccessor.HttpContext?.User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier) ?? false;
            if (!isAuthenticated)
            {
                return RedirectToPage("../login");
            }
            else
            {
                var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "00000000-0000-0000-0000-000000000000";
                UserId = new Guid(userId);
                Token = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "token")?.Value ?? "";
            }

            var errors = new List<string>();
            if (CoffeeShopJson is null)
            {
                errors.Add("Coffee Shop information not found");
            }
            if (AreaJson is null)
            {
                errors.Add("Area information not found");
            }
            if (TimeFrameId is null)
            {
                errors.Add("Time frame not found");
            }
            if (BookedDate is null)
            {
                errors.Add("Booked date not found");
            }
            if (BookedSlots is null)
            {
                errors.Add("Booked slots not found");
            }
            if (!errors.IsNullOrEmpty())
            {
                ViewData["warning"] = "Can not load data";
                ViewData["errors"] = errors;
                return Page();
            }

            var shop = JsonSerializer.Deserialize<CoffeeShopResponseDTO>(CoffeeShopJson ?? "");
            var area = JsonSerializer.Deserialize<AreaDto>(AreaJson ?? "");
            var timeFrameResponse = await _apiTimeFrame.GetAsync<ResponseBody<TimeFrameDto>>(TimeFrameId.ToString() ?? "");
            var timeFrame = timeFrameResponse?.Result;
            IList<BusinessObject.Model.Product> products = new List<BusinessObject.Model.Product>();

            var productsResponse = await client.GetAsync($"{ProductUrl}?shopId={shop?.CoffeeShopId ?? 0}");
            string productsResponseMessage = await productsResponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            if (productsResponse.IsSuccessStatusCode)
            {
                products = JsonSerializer.Deserialize<List<BusinessObject.Model.Product>>(productsResponseMessage, options) ?? Enumerable.Empty<BusinessObject.Model.Product>().ToList();
            }

            if (shop is null || area is null || timeFrame is null)
            {
                ViewData["warning"] = "Load data failed";
                return Page();
            }

            if (!TimeOnly.TryParse(timeFrame.StartTime, out var startTime)
                || !TimeOnly.TryParse(timeFrame.EndTime, out var endTime))
            {
                ViewData["warning"] = "Load bill failed";
                return Page();
            }
            else
            {
                var bookedDuration = (endTime - startTime).TotalMinutes;
                var pricePerHour = area.PricePerHour;
                var test = (double)pricePerHour * (bookedDuration / 60) * BookedSlots;
                if (decimal.TryParse(test.ToString(), out var totalRentalPrice))
                {
                    TotalRentalPrice = totalRentalPrice;
                }
                else
                {
                    ViewData["warning"] = "Load bill failed";
                    return Page();
                }
            }

            CoffeeShop = shop;
            Area = area;
            TimeFrame = timeFrame;
            Products = products;

            return Page();
        }

        public async Task<IActionResult> OnPostPayOrderAsync()
        {
            var apiResponse = await _apiBooking.GetAsync<ResponseBody<BookingResponseDTO>>(BookingId.ToString() ?? "0");
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
    }
}
