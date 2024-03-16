using CatCoffeePlatformRazorPages.Common;
using DTO.CoffeeShopDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace CatCoffeePlatformRazorPages.Pages.CoffeeShopPages
{
    public class IndexModel : PageModel
    {
        private readonly ApiHelper _apiShop;
        private IHttpContextAccessor httpContextAccessor;
        public IndexModel(IHttpContextAccessor httpContextAccessor)
        {
            _apiShop = new ApiHelper(ApiResources.CoffeeShops);
            this.httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<CoffeeShopResponseDTO> CoffeeShop { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var isAuthenticated = httpContextAccessor.HttpContext?.User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier) ?? false;
            if (!isAuthenticated)
            {
                return RedirectToPage("../login");
            }
            var apiResponse = await _apiShop.GetAsync<ResponseBody<IEnumerable<CoffeeShopResponseDTO>>>();
            var shopList = apiResponse!.Result;
            if (shopList is not null)
            {
                CoffeeShop = shopList;
                return Page();
            }
            return NotFound();
        }
    }
}
