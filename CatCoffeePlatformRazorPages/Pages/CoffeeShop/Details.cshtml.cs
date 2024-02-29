// ... other using statements ...

using CatCoffeePlatformRazorPages.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CatCoffeePlatformRazorPages.Pages.CoffeeShop
{
    public class DetailsModel : PageModel
    {
        private readonly ApiHelper _apiCoffeeShop;

        public DetailsModel()
        {
            _apiCoffeeShop = new ApiHelper(ApiResources.CoffeeShops);
        }

        public BusinessObject.Model.CoffeeShop CoffeeShop { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jsonOptions = new JsonSerializerOptions
            {
                Converters = {new TimeOnlyConverter() },
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            };

            var apiResponse = await _apiCoffeeShop.GetAsync<ResponseBody<BusinessObject.Model.CoffeeShop>>($"{id}");
            var coffeeShopResponse = apiResponse!.Result;

            if (coffeeShopResponse == null)
            {
                return NotFound();
            }
            else
            {
                // Deserialize using JsonSerializer with custom TimeOnlyConverter
                var coffeeShopJson = JsonSerializer.Serialize(coffeeShopResponse, jsonOptions);
                CoffeeShop = JsonSerializer.Deserialize<BusinessObject.Model.CoffeeShop>(coffeeShopJson, jsonOptions);
            }

            return Page();
        }
    }
}
