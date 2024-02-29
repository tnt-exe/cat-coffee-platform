
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
        public async Task<IActionResult> OnGet()
        {
            return Page();
        }
    }
}
