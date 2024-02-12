using CatCoffeePlatformRazorPages.Common;
using DTO.AreaDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages.AreaPages
{
    public class EditModel : PageModel
    {
        private readonly ApiHelper _apiArea;

        public EditModel()
        {
            _apiArea = new ApiHelper(ApiResources.Areas);
        }

        [BindProperty]
        public AreaUpdate Area { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _apiArea.GetAsync<AreaUpdate>($"{id}");
            if (area == null)
            {
                return NotFound();
            }
            Area = area;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool result = await _apiArea.PutAsync($"{Area.AreaId}", Area);

            if (!result)
            {
                return Page();
            }

            TempData["area-msg"] = "Update area success";
            return RedirectToPage("./Index");
        }
    }
}
