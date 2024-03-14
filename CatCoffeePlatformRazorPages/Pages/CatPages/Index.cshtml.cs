﻿using CatCoffeePlatformRazorPages.Common;
using DTO.CatDTO;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages.CatPages
{
    public class IndexModel : PageModel
    {
        private readonly ApiHelper _apiCat;

        public IndexModel()
        {
            _apiCat = new ApiHelper(ApiResources.Cats);
        }

        public IEnumerable<CatDto> Cat { get; set; } = default!;

        public async Task OnGetAsync(int? shopId)
        {
            var apiResponse = await _apiCat.GetAsync<ResponseBody<IEnumerable<CatDto>>>();
            var catList = apiResponse!.Result;

            if (catList is not null)
            {
                if (shopId != null)
                {
                    catList = catList
                        .Where(x => x.CoffeeShopId == shopId);
                }

                Cat = catList;
            }
        }
    }
}
