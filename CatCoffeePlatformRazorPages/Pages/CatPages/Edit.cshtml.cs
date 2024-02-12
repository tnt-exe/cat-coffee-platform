using BusinessObject.Enums;
using CatCoffeePlatformRazorPages.Common;
using DTO.AreaDTO;
using DTO.CatDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CatCoffeePlatformRazorPages.Pages.CatPages
{
    public class EditModel : PageModel
    {
        private readonly ApiHelper _apiCat;
        private readonly ApiHelper _apiArea;

        public EditModel()
        {
            _apiCat = new ApiHelper(ApiResources.Cats);
            _apiArea = new ApiHelper(ApiResources.Areas);
        }

        [BindProperty]
        public CatUpdate Cat { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat = await _apiCat.GetAsync<CatUpdate>($"{id}");
            if (cat == null)
            {
                return NotFound();
            }
            Cat = cat;

            await _loadSelectList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await _loadSelectList();
                return Page();
            }

            bool result = await _apiCat.PutAsync($"{Cat.CatId}", Cat);

            if (!result)
            {
                await _loadSelectList();
                return Page();
            }

            TempData["cat-msg"] = "Update cat success";
            return RedirectToPage("./Index");
        }

        private async Task _loadSelectList()
        {
            var areaList = await _apiArea.GetAsync<IEnumerable<AreaDto>>();
            var statusList = from CatStatus catStatus in Enum.GetValues(typeof(CatStatus))
                             select new
                             {
                                 HealthyStatus = (int)catStatus,
                                 StatusName = catStatus.ToString()
                             };

            ViewData["AreaId"] = new SelectList(areaList, "AreaId", "AreaName");
            ViewData["CatStatus"] = new SelectList(statusList, "HealthyStatus", "StatusName");
        }
    }
}
