using CatCoffeePlatformRazorPages.Common;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages.CategoryPages
{
    public class CreateModel : PageModel
    {
        private readonly ApiHelper _apiCategory;

        public CreateModel()
        {
            _apiCategory = new ApiHelper(ApiResources.Categories);
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CategoryCreate Category { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool result = await _apiCategory.PostAsync(Category);

            if (!result)
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
