using BusinessObject.Enums;
using CatCoffeePlatformRazorPages.Common;
using DTO.AreaDTO;
using DTO.CatDTO;
using DTO.CoffeeShopDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CatCoffeePlatformRazorPages.Pages.CatPages
{
    public class CreateModel : PageModel
    {
        private readonly ApiHelper _apiCat;
        private readonly ApiHelper _apiArea;
        private readonly ApiHelper _apiCoffeeShop;

        public CreateModel()
        {
            _apiCat = new ApiHelper(ApiResources.Cats);
            _apiArea = new ApiHelper(ApiResources.Areas);
            _apiCoffeeShop = new ApiHelper(ApiResources.CoffeeShops);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await _loadSelectList();
            return Page();
        }

        [BindProperty]
        public CatCreate Cat { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await _loadSelectList();
                return Page();
            }

            bool result = await _apiCat.PostAsync(Cat);

            if (!result)
            {
                await _loadSelectList();
                return Page();
            }

            TempData["cat-msg"] = "Create cat success";
            return RedirectToPage("./Index");
        }

        private async Task _loadSelectList()
        {
            var apiResponse = await _apiArea.GetAsync<ResponseBody<IEnumerable<AreaDto>>>();
            var areaList = apiResponse!.Result;

            var statusList = from CatStatus catStatus in Enum.GetValues(typeof(CatStatus))
                             select new
                             {
                                 HealthyStatus = (int)catStatus,
                                 StatusName = catStatus.ToString()
                             };

            var coffeeShopResponse = await _apiCoffeeShop
                .GetAsync<ResponseBody<IEnumerable<CoffeeShopResponseDTO>>>();
            var coffeeShopList = coffeeShopResponse!.Result;

            if (areaList is not null)
            {
                ViewData["AreaId"] = new SelectList(areaList, "AreaId", "AreaName");
            }
            if (coffeeShopList is not null)
            {
                ViewData["CoffeeShopId"] = new SelectList(coffeeShopList, "CoffeeShopId", "ShopName");
            }
            ViewData["CatStatus"] = new SelectList(statusList, "HealthyStatus", "StatusName");
        }
    }
}
