using CatCoffeePlatformRazorPages.Common;
using DTO.CoffeeShopDTO;
using DTO.TimeFrameDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CatCoffeePlatformRazorPages.Pages.TimeFramePages
{
    public class CreateModel : PageModel
    {
        private readonly ApiHelper _apiTimeFrame;
        private readonly ApiHelper _apiCoffeeShop;

        public CreateModel()
        {
            _apiTimeFrame = new ApiHelper(ApiResources.TimeFrames);
            _apiCoffeeShop = new ApiHelper(ApiResources.CoffeeShops);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await _loadSelectList();
            return Page();
        }

        [BindProperty]
        public TimeFrameCreate TimeFrame { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await _loadSelectList();
                return Page();
            }

            bool result = await _apiTimeFrame.PostAsync(TimeFrame);

            if (!result)
            {
                await _loadSelectList();
                return Page();
            }

            TempData["tf-msg"] = "Create time frame success";
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
