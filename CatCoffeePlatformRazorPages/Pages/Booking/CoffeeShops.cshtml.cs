using Microsoft.AspNetCore.Mvc.RazorPages;
using CatCoffeePlatformRazorPages.Common;
using DTO.CoffeeShopDTO;
using Microsoft.AspNetCore.Mvc;

namespace CatCoffeePlatformRazorPages.Pages.Booking
{
    public class CoffeeShopsModel : PageModel
    {
        private readonly ApiHelper _apiCoffeeShop;

        public CoffeeShopsModel()
        {
            _apiCoffeeShop = new ApiHelper(ApiResources.CoffeeShops);
        }

        public IList<CoffeeShopResponseDTO> CoffeeShops { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var apiResponse = await _apiCoffeeShop
                .GetAsync<ResponseBody<IEnumerable<CoffeeShopResponseDTO>>>();
            CoffeeShops = apiResponse?.Result?.ToArray() ?? new CoffeeShopResponseDTO[0];
            return Page();
        }
    }
}
