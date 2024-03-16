using BusinessObject.Enums;
using CatCoffeePlatformRazorPages.Common;
using DTO.AreaDTO;
using DTO.CatDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CatCoffeePlatformRazorPages.Pages.CatPages
{
    public class CreateModel : PageModel
    {
        private readonly ApiHelper _apiCat;
        private readonly ApiHelper _apiArea;

        public CreateModel()
        {
            _apiCat = new ApiHelper(ApiResources.Cats);
            _apiArea = new ApiHelper(ApiResources.Areas);
        }

        public async Task<IActionResult> OnGetAsync(int? shopId)
        {
            if (shopId == null)
            {
                return NotFound();
            }

            ViewData["shopId"] = shopId;
            ShopId = shopId.Value;

            await LoadSelectList();
            return Page();
        }

        [BindProperty]
        public CatCreate Cat { get; set; } = default!;

        [BindProperty]
        public int ShopId { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectList();
                return Page();
            }

            bool result = await _apiCat.PostAsync(Cat);

            if (!result)
            {
                await LoadSelectList();
                return Page();
            }

            TempData["cat-msg"] = "Create cat success";

            return Redirect("/CoffeeShopPages/Details?id=" + ShopId);
        }

        private async Task LoadSelectList()
        {
            var apiResponse = await _apiArea.GetAsync<ResponseBody<IEnumerable<AreaDto>>>();
            var areaList = apiResponse!.Result!
                .Where(a => a.CoffeeShopId == ShopId);

            var statusList = from CatStatus catStatus in Enum.GetValues(typeof(CatStatus))
                             select new
                             {
                                 HealthyStatus = (int)catStatus,
                                 StatusName = catStatus.ToString()
                             };

            if (areaList.Any())
            {
                ViewData["AreaId"] = new SelectList(areaList, "AreaId", "AreaName");
            }
            ViewData["CatStatus"] = new SelectList(statusList, "HealthyStatus", "StatusName");
        }
    }
}
