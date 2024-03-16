using CatCoffeePlatformRazorPages.Common;
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

        public async Task<IActionResult> OnGetAsync(int? catId, int? shopId)
        {
            if (catId == null || shopId == null)
            {
                return NotFound();
            }

            var apiResponse = await _apiCat.GetAsync<ResponseBody<CatDto>>($"{catId}");
            var cat = apiResponse!.Result;
            if (cat == null)
            {
                return NotFound();
            }
            else
            {
                Cat = cat;
            }

            ViewData["shopId"] = shopId;

            return Page();
        }
    }
}
