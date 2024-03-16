using CatCoffeePlatformRazorPages.Common;
using DTO.CoffeeShopDTO;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages.CoffeeShopPages
{
    public class ShowAllCoffeeShopModel : PageModel
    {
        private readonly ApiHelper _apiShop;

        public ShowAllCoffeeShopModel()
        {
            _apiShop = new ApiHelper(ApiResources.CoffeeShops);
        }

        public IEnumerable<CoffeeShopResponseDTO> CoffeeShop { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var apiResponse = await _apiShop.GetAsync<ResponseBody<IEnumerable<CoffeeShopResponseDTO>>>();
            var shopList = apiResponse!.Result;
            if (shopList is not null)
            {
                CoffeeShop = shopList;
            }
        }
    }
}
