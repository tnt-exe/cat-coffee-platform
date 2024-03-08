
using CatCoffeePlatformRazorPages.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace CatCoffeePlatformRazorPages.Pages.Booking
{
    public class StripeSuccess : PageModel
    {
        private readonly StripeSetting _stripeSettings;
        public StripeSuccess(IOptions<StripeSetting> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
        }

        [BindProperty]
        public int BookingId { get; set; }

        public async Task<IActionResult> OnGet(int BookingId)
        {
            this.BookingId = BookingId;
            return Page();
        }
    }
}
