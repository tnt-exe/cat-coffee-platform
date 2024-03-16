using CatCoffeePlatformRazorPages.Common;
using DTO.TimeFrameDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages.TimeFramePages
{
    public class EditModel : PageModel
    {
        private readonly ApiHelper _apiTimeFrame;

        public EditModel()
        {
            _apiTimeFrame = new ApiHelper(ApiResources.TimeFrames);
        }

        [BindProperty]
        public TimeFrameUpdate TimeFrame { get; set; } = default!;

        [BindProperty]
        public int ShopId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? tfId, int? shopId)
        {
            if (tfId == null || shopId == null)
            {
                return NotFound();
            }

            var apiResponse = await _apiTimeFrame.GetAsync<ResponseBody<TimeFrameUpdate>>($"{tfId}");
            var timeFrame = apiResponse!.Result;
            if (timeFrame == null)
            {
                return NotFound();
            }
            TimeFrame = timeFrame;

            ViewData["shopId"] = shopId;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool result = await _apiTimeFrame
                 .PutAsync($"{TimeFrame.TimeFrameId}", TimeFrame);

            if (!result)
            {
                return Page();
            }

            TempData["tf-msg"] = "Update timeframe success";

            return Redirect("/CoffeeShopPages/Details?id=" + ShopId);
        }
    }
}
