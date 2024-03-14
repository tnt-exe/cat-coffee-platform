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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ResponseBody<AreaDto>? areaResponse = await _apiArea.GetAsync<ResponseBody<AreaDto>>($"{id}");
            IEnumerable<CatDto>? catList = await _apiCat.GetAsync<ResponseBody<IEnumerable<CatDto>>>()
                .ContinueWith(t => t.Result?.Result?.Where(c => c.AreaId == id));

            if (areaResponse != null)
            {
                Area = areaResponse.Result!;
                Cats = catList;
            } 
            else 
            {
                return NotFound();
            }

            return Page();
        }
    }
}
