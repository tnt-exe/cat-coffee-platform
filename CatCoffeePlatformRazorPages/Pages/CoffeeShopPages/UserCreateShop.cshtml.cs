using CatCoffeePlatformRazorPages.Common;
using DTO.CoffeeShopDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace CatCoffeePlatformRazorPages.Pages.CoffeeShopPages
{
    public class UserCreateShopModel : PageModel
    {
        private readonly ApiHelper _apiShop;
        private IHttpContextAccessor _httpContextAccessor;
        public UserCreateShopModel(IHttpContextAccessor httpContextAccessor)
        {
            _apiShop = new ApiHelper(ApiResources.CoffeeShops);
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CoffeeShopCreate CoffeeShop { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var idString = _httpContextAccessor.HttpContext?.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;

            var id = new Guid(idString!);
            var newShop = new CoffeeShopCreate
            {
                ShopName = CoffeeShop.ShopName,
                Address = CoffeeShop.Address,
                OpeningTime = CoffeeShop.OpeningTime,
                ClosingTime = CoffeeShop.ClosingTime,
                ContactNumber = CoffeeShop.ContactNumber,
                Email = CoffeeShop.Email,
                Description = CoffeeShop.Description,
                ManagerId = id
            };
            var result = await _apiShop.PostAsync(newShop);

            if (!result)
            {
                TempData["shop-msg"] = "Create shop success";
                return RedirectToPage("../home");
            }
            return Page();
        }

    }
}

