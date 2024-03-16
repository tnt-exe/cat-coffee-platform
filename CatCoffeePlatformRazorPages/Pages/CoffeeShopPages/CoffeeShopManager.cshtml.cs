using CatCoffeePlatformRazorPages.Common;
using DTO.CoffeeShopDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages.CoffeeShopPages
{
    public class CoffeeShopManagerModel : PageModel
    {
        private readonly ApiHelper _apiShop;

        public CoffeeShopManagerModel()
        {
            _apiShop = new ApiHelper(ApiResources.CoffeeShops);
        }
        public CoffeeShopResponseDTO CoffeeShop { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var apiResponse = await _apiShop.GetAsync<ResponseBody<CoffeeShopResponseDTO>>($"get-by-manager-id/{id}");
            var shop = apiResponse!.Result;

            if (shop == null)
            {
                return NotFound();
            }
            else
            {
                CoffeeShop = shop;
            }
            return Page();
        }

        public async Task<IActionResult> OnGetCoffeeShopAsync(int CoffeeShopId)
        {
            var apiResponse = await _apiShop.GetAsync<ResponseBody<CoffeeShopResponseDTO>>(CoffeeShopId.ToString());
            var shop = apiResponse!.Result;

            if (shop == null)
            {
                return NotFound();
            }
            else
            {
                CoffeeShop = shop;
            }
            return Page();
        }
    }
}
