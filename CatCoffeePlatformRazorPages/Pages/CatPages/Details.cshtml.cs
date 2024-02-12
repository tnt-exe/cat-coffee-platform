﻿using CatCoffeePlatformRazorPages.Common;
using DTO.CatDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages.CatPages
{
    public class DetailsModel : PageModel
    {
        private readonly ApiHelper _apiCat;

        public DetailsModel()
        {
            _apiCat = new ApiHelper(ApiResources.Cats);
        }

        public CatDto Cat { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat = await _apiCat.GetAsync<CatDto>($"{id}");
            if (cat == null)
            {
                return NotFound();
            }
            else
            {
                Cat = cat;
            }
            return Page();
        }
    }
}