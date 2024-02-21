using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Model;
using DAO.Context;
using CatCoffeePlatformRazorPages.Common;
using DTO.CoffeeShopDTO;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CatCoffeePlatformRazorPages.Pages.CoffeeShopPages
{
    public class CreateModel : PageModel
    {
        private readonly ApiHelper _apiShop;
        public CreateModel()
        {
            _apiShop = new ApiHelper(ApiResources.CoffeeShops);
        }

        [BindProperty]
        public CoffeeShopCreate CoffeeShop { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //var idString = _httpContextAccessor.HttpContext?.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;

            //var id = new Guid(idString!);
            var newShop = new CoffeeShopCreate
            {
                ShopName = CoffeeShop.ShopName,
                Address = CoffeeShop.Address,
                OpeningTime = CoffeeShop.OpeningTime,
                ClosingTime = CoffeeShop.ClosingTime,
                ContactNumber = CoffeeShop.ContactNumber,
                Email = CoffeeShop.Email,
                Description = CoffeeShop.Description,
                ManagerId = CoffeeShop.ManagerId,
            };
            var result = await _apiShop.PostAsync(newShop);

            if (!result)
            {
                return RedirectToPage("./Index");
            }
            return Page();
        }
    }
}
