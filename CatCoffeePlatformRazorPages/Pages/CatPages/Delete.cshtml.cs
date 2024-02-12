using CatCoffeePlatformRazorPages.Common;
using DTO.CatDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages.CatPages
{
    public class DeleteModel : PageModel
    {
        private readonly ApiHelper _apiCat;

        public DeleteModel()
        {
            _apiCat = new ApiHelper(ApiResources.Cats);
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _apiCat.DeleteAsync($"{id}");
            if (!result)
            {
                TempData["cat-msg"] = "Delete fail";
                return RedirectToPage("./Index");
            }

            TempData["cat-msg"] = "Delete cat success";
            return RedirectToPage("./Index");
        }
    }
}
