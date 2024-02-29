using Microsoft.AspNetCore.Mvc.RazorPages;
using CatCoffeePlatformRazorPages.Common;
using DTO.CoffeeShopDTO;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CatCoffeePlatformRazorPages.Pages.Booking
{
    public class CoffeeShopsModel : PageModel
    {
        private readonly ApiHelper _apiCoffeeShop;
        private IHttpContextAccessor httpContextAccessor;

        public CoffeeShopsModel(IHttpContextAccessor httpContextAccessor)
        {
            _apiCoffeeShop = new ApiHelper(ApiResources.CoffeeShops);
            this.httpContextAccessor = httpContextAccessor;
        }

        public IList<CoffeeShopResponseDTO> CoffeeShops { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var isAuthenticated = httpContextAccessor.HttpContext?.User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier) ?? false;
            if (!isAuthenticated)
            {
                return RedirectToPage("../login");
            }

            var apiResponse = await _apiCoffeeShop
                .GetAsync<ResponseBody<IEnumerable<CoffeeShopResponseDTO>>>();
            CoffeeShops = apiResponse?.Result?.ToArray() ?? new CoffeeShopResponseDTO[0];
            return Page();
        }
    }
}
