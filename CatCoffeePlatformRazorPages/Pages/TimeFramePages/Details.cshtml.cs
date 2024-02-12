using CatCoffeePlatformRazorPages.Common;
using DTO.TimeFrameDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages.TimeFramePages
{
    public class DetailsModel : PageModel
    {
        private readonly ApiHelper _apiTimeFrame;

        public DetailsModel()
        {
            _apiTimeFrame = new ApiHelper(ApiResources.TimeFrames);
        }

        public TimeFrameDto TimeFrame { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeframe = await _apiTimeFrame.GetAsync<TimeFrameDto>($"{id}");
            if (timeframe == null)
            {
                return NotFound();
            }
            else
            {
                TimeFrame = timeframe;
            }
            return Page();
        }
    }
}
