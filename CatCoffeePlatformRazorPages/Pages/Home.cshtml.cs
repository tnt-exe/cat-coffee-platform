using BusinessObject.Model;
using CatCoffeePlatformRazorPages.Common;
using DTO.AreaDTO;
using DTO.CoffeeShopDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages
{
    public class HomeModel : PageModel
    {
        private readonly ILogger<HomeModel> _logger;
        private readonly ApiHelper _apiCoffeeShop;

        public HomeModel(ILogger<HomeModel> logger)
        {
            _logger = logger;
            _apiCoffeeShop = new ApiHelper(ApiResources.CoffeeShops);
        }

        public IEnumerable<CoffeeShopResponseDTO> CoffeeShops { get; set; } = default!;

/*        public async Task OnGetAsync()
        {
            var apiResponse = await _apiCoffeeShop
                .GetAsync<ResponseBody<IEnumerable<CoffeeShopResponseDTO>>>();
            var list = apiResponse!.Result;

            if (list is not null)
            {
                CoffeeShops = list;
            }
        }*/
    }
}