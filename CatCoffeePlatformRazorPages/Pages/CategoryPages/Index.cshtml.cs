using Microsoft.AspNetCore.Mvc.RazorPages;
using CatCoffeePlatformRazorPages.Common;
using DTO;

namespace CatCoffeePlatformRazorPages.Pages.CategoryPages
{
    public class IndexModel : PageModel
    {
        private readonly ApiHelper _apiCategory;

        public IndexModel()
        {
            _apiCategory = new ApiHelper(ApiResources.Categories);
        }

        public IEnumerable<CategoryDto> Category { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var apiResponse = await _apiCategory
                .GetAsync<ResponseBody<IEnumerable<CategoryDto>>>();

            var categoryList = apiResponse!.Result;

            if (categoryList is not null)
            {
                Category = categoryList;
            }
        }
    }
}
