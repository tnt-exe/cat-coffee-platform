using CatCoffeePlatformRazorPages.Common;
using DTO.AreaDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages.AreaPages
{
    public class DetailsModel : PageModel
    {
        private readonly ApiHelper _apiArea;

        public DetailsModel()
        {
            _apiArea = new ApiHelper(ApiResources.Areas);
        }

        public AreaDto Area { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _apiArea.GetAsync<AreaDto>($"{id}");
            if (area == null)
            {
                return NotFound();
            }
            else
            {
                Area = area;
            }
            return Page();
        }
    }
}
