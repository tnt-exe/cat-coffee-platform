using CatCoffeePlatformRazorPages.Common;
using DTO.TimeFrameDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages.TimeFramePages
{
    public class IndexModel : PageModel
    {
        private readonly ApiHelper _apiTimeFrame;

        public IndexModel()
        {
            _apiTimeFrame = new ApiHelper(ApiResources.TimeFrames);
        }

        public IEnumerable<TimeFrameDto> TimeFrame { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? shopId)
        {
            if (shopId == null)
            {
                return NotFound();
            }

            var apiResponse = await _apiTimeFrame
                .GetAsync<ResponseBody<IEnumerable<TimeFrameDto>>>();
            var timeFrameList = apiResponse!.Result;

            if (timeFrameList == null)
            {
                return NotFound();
            }

            timeFrameList = timeFrameList
                    .Where(x => x.CoffeeShopId == shopId);

            TimeFrame = timeFrameList;

            ViewData["shopId"] = shopId;

            return Page();
        }
    }
}
