using CatCoffeePlatformRazorPages.Common;
using DTO.AreaDTO;
using DTO.CatDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages.AreaPages
{
    public class DetailsModel : PageModel
    {
        private readonly ApiHelper _apiArea;
        private readonly ApiHelper _apiCat;

        public DetailsModel()
        {
            _apiArea = new ApiHelper(ApiResources.Areas);
            _apiCat = new ApiHelper(ApiResources.Cats);
        }

        public AreaDto Area { get; set; } = default!;
        public IEnumerable<CatDto>? Cats { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? areaId, int? shopId)
        {
            if (areaId == null || shopId == null)
            {
                return NotFound();
            }

            ResponseBody<AreaDto>? areaResponse = await _apiArea.GetAsync<ResponseBody<AreaDto>>($"{areaId}");
            IEnumerable<CatDto>? catList = await _apiCat.GetAsync<ResponseBody<IEnumerable<CatDto>>>()
                .ContinueWith(t => t.Result?.Result?.Where(c => c.AreaId == areaId));

            if (areaResponse == null)
            {
                return NotFound();
            }

            Area = areaResponse.Result!;
            Cats = catList;

            ViewData["shopId"] = shopId;

            return Page();
        }
    }
}
