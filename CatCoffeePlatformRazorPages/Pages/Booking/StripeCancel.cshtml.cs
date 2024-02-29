
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using CatCoffeePlatformRazorPages.Common;

namespace CatCoffeePlatformRazorPages.Pages.Booking
{
    public class StripeCancel : PageModel
    {
        private readonly StripeSetting _stripeSettings;
        public StripeCancel(IOptions<StripeSetting> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
        }
        public string SessionId { get ; set; }

        [BindProperty]
        public Guid OrderItemId { get; set; }

        [BindProperty]
        public decimal Price { get; set; }
        public async Task<IActionResult> OnGet()
        {
            return Page(); // Return to the original page after handling the payment success
        }
    }
}
