using CatCoffeePlatformRazorPages.Common;
using DTO.AreaDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public AreaCreate Area { get; set; } = default!;

        [BindProperty]
        public int ShopId { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool result = await _apiArea.PostAsync(Area);

            if (!result)
            {
                return Page();
            }

            TempData["area-msg"] = "Create area success";

            return Redirect("/CoffeeShopPages/Details?id=" + ShopId);
        }
    }
}
