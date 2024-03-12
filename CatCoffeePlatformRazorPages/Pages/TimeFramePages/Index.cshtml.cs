﻿using CatCoffeePlatformRazorPages.Common;
using DTO.TimeFrameDTO;
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

        public async Task OnGetAsync(int? shopId)
        {
            var apiResponse = await _apiTimeFrame
                .GetAsync<ResponseBody<IEnumerable<TimeFrameDto>>>();
            var timeFrameList = apiResponse!.Result;

            if (timeFrameList is not null)
            {
                if (shopId is not null)
                {
                    timeFrameList = timeFrameList
                        .Where(x => x.CoffeeShopId == shopId);
                }

                TimeFrame = timeFrameList;
            }
        }
    }
}
