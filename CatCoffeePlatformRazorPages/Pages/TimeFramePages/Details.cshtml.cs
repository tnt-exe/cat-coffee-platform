﻿using CatCoffeePlatformRazorPages.Common;
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

            var apiResponse = await _apiTimeFrame.GetAsync<ResponseBody<TimeFrameDto>>($"{id}");
            var timeFrame = apiResponse!.Result;
            if (timeFrame == null)
            {
                return NotFound();
            }
            else
            {
                TimeFrame = timeFrame;
            }
            return Page();
        }
    }
}
