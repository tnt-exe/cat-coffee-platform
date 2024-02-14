using CatCoffeePlatformRazorPages.Common;
using DTO.AreaDTO;
using DTO.CoffeeShopDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CatCoffeePlatformRazorPages.Pages.AreaPages
{
    public class CreateModel : PageModel
    {
        private readonly ApiHelper _apiArea;
        private readonly ApiHelper _apiCoffeeShop;

        public CreateModel()
        {
            _apiArea = new ApiHelper(ApiResources.Areas);
            _apiCoffeeShop = new ApiHelper(ApiResources.CoffeeShops);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await _loadSelectList();
            return Page();
        }

        [BindProperty]
        public AreaCreate Area { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await _loadSelectList();
                return Page();
            }

            bool result = await _apiArea.PostAsync(Area);

            if (!result)
            {
                await _loadSelectList();
                return Page();
            }

            TempData["area-msg"] = "Create area success";
            return RedirectToPage("./Index");
        }

        private async Task _loadSelectList()
        {
            var apiResponse = await _apiCoffeeShop
                .GetAsync<ResponseBody<IEnumerable<CoffeeShopResponseDTO>>>();
            var coffeeShopList = apiResponse!.Result;

            if (coffeeShopList is not null)
            {
                ViewData["CoffeeShopId"] = new SelectList(coffeeShopList, "CoffeeShopId", "ShopName");
            }
        }
    }
}
