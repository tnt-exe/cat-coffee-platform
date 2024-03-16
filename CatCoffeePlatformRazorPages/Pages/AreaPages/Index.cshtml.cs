using CatCoffeePlatformRazorPages.Common;
using DTO.AreaDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages.AreaPages
{
    public class IndexModel : PageModel
    {
        private readonly ApiHelper _apiArea;

        public IndexModel()
        {
            _apiArea = new ApiHelper(ApiResources.Areas);
        }

        public IEnumerable<AreaDto> Area { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? shopId)
        {
            if (shopId == null)
            {
                return NotFound();
            }

            var apiResponse = await _apiArea
                .GetAsync<ResponseBody<IEnumerable<AreaDto>>>();
            var areaList = apiResponse!.Result;

            if (areaList == null)
            {
                return NotFound();
            }

            areaList = areaList
                .Where(x => x.CoffeeShopId == shopId);

            Area = areaList;

            ViewData["shopId"] = shopId;

            return Page();
        }
    }
}
