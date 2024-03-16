using CatCoffeePlatformRazorPages.Common;
using DTO.AreaDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages.AreaPages
{
    public class EditModel : PageModel
    {
        private readonly ApiHelper _apiArea;

        public EditModel()
        {
            _apiArea = new ApiHelper(ApiResources.Areas);
        }

        [BindProperty]
        public AreaUpdate Area { get; set; } = default!;

        [BindProperty]
        public int ShopId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? areaId, int? shopId)
        {
            if (areaId == null || shopId == null)
            {
                return NotFound();
            }

            var apiResponse = await _apiArea.GetAsync<ResponseBody<AreaUpdate>>($"{areaId}");
            var area = apiResponse!.Result;
            if (area == null)
            {
                return NotFound();
            }
            Area = area;

            ViewData["shopId"] = shopId;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool result = await _apiArea.PutAsync($"{Area.AreaId}", Area);

            if (!result)
            {
                return Page();
            }

            TempData["area-msg"] = "Update area success";

            return Redirect("/CoffeeShopPages/Details?id=" + ShopId);
        }
    }
}
