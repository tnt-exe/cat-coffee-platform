
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using CatCoffeePlatformRazorPages.Common;
using DTO.BookingDTO;
using BusinessObject.Enums;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CatCoffeePlatformRazorPages.Pages.Booking
{
    public class StripeCancel : PageModel
    {
        private readonly StripeSetting _stripeSettings;
        private readonly HttpClient client = null!;
        private string BookingApiUrl = "";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StripeCancel(IOptions<StripeSetting> stripeSettings, IHttpContextAccessor httpContextAccessor)
        {
            _stripeSettings = stripeSettings.Value;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BookingApiUrl = "https://localhost:7039/api/Bookings";
            _httpContextAccessor = httpContextAccessor;
        }
        public string SessionId { get ; set; }

        [BindProperty]
        public Guid OrderItemId { get; set; }

        [BindProperty]
        public decimal Price { get; set; }

        public async Task<IActionResult> OnGet(int BookingId)
        {
            var token = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "token")?.Value ?? "";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string strData = JsonSerializer.Serialize(new
            {
                Status = (int)BookingStatus.None,
                PaymentStatus = (int)PaymentStatus.UnPaid
            });
            var contentData = new StringContent(strData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync($"{BookingApiUrl}/{BookingId}", contentData);
            string responseMessage = await response.Content.ReadAsStringAsync();
            var status = response.StatusCode.ToString();

            return Page(); // Return to the original page after handling the payment success
        }
    }
}
