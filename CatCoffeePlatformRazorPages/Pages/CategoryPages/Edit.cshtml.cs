using CatCoffeePlatformRazorPages.Common;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages.CategoryPages
{
    public class EditModel : PageModel
    {
        private readonly ApiHelper _apiCategory;

        public EditModel()
        {
            _apiCategory = new ApiHelper(ApiResources.Categories);
        }

        [BindProperty]
        public CategoryUpdate Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiResponse = await _apiCategory.GetAsync<ResponseBody<CategoryUpdate>>($"{id}");
            var category = apiResponse!.Result;
            if (category == null)
            {
                return NotFound();
            }
            Category = category;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool result = await _apiCategory.PutAsync($"{Category.CategoryId}", Category);

            if (!result)
            {
                return Page();
            }

            TempData["category-msg"] = "Update category success";
            return RedirectToPage("./Index");
        }
    }
}
