using CatCoffeePlatformRazorPages.Common;
using DTO.CatDTO;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> OnGetAsync(int? shopId)
        {
            if (shopId == null)
            {
                return NotFound();
            }

            var apiResponse = await _apiCat.GetAsync<ResponseBody<IEnumerable<CatDto>>>();
            var catList = apiResponse!.Result;

            if (catList == null)
            {
                return NotFound();
            }

            catList = catList
                    .Where(x => x.CoffeeShopId == shopId);

            Cat = catList;

            ViewData["shopId"] = shopId;

            return Page();
        }
    }
}
