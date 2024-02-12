﻿using CatCoffeePlatformRazorPages.Common;
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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeframe = await _apiTimeFrame.GetAsync<TimeFrameUpdate>($"{id}");
            if (timeframe == null)
            {
                return NotFound();
            }
            TimeFrame = timeframe;

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
            return RedirectToPage("./Index");
        }
    }
}