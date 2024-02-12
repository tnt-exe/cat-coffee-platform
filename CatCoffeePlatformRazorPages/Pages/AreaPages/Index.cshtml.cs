using CatCoffeePlatformRazorPages.Common;
using DTO.AreaDTO;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages.AreaPages
{
    public class IndexModel : PageModel
    {
        private readonly ApiHelper _apiArea;

        public IndexModel()
        {
            _apiArea = new ApiHelper(ApiResources.Areas);
        }

        public IEnumerable<AreaDto> Area { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var areaList = await _apiArea
                .GetAsync<IEnumerable<AreaDto>>();
            if (areaList is not null)
            {
                Area = areaList;
            }
        }
    }
}
