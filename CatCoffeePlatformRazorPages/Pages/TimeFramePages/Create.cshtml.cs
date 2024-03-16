using CatCoffeePlatformRazorPages.Common;
using DTO.TimeFrameDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public async Task<IActionResult> OnGetAsync(int? shopId)
        {
            if (shopId == null)
            {
                return NotFound();
            }

            ViewData["shopId"] = shopId;

            return Page();
        }

        [BindProperty]
        public TimeFrameCreate TimeFrame { get; set; } = default!;

        [BindProperty]
        public int ShopId { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool result = await _apiTimeFrame.PostAsync(TimeFrame);

            if (!result)
            {
                return Page();
            }

            TempData["tf-msg"] = "Create time frame success";

            return Redirect("/CoffeeShopPages/Details?id=" + ShopId);
        }
    }
}
