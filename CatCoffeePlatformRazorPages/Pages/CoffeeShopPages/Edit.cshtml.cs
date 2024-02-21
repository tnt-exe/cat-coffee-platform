using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;
using DAO.Context;
using DTO.CoffeeShopDTO;
using CatCoffeePlatformRazorPages.Common;

namespace CatCoffeePlatformRazorPages.Pages.CoffeeShopPages
{
    public class EditModel : PageModel
    {
        private readonly ApiHelper _apiShop;

        public EditModel()
        {
            _apiShop = new ApiHelper(ApiResources.CoffeeShops);
        }

        [BindProperty]
        public CoffeeShopResponseDTO CoffeeShop { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiResponse = await _apiShop.GetAsync<ResponseBody<CoffeeShopResponseDTO>>($"{id}");
            var coffeeshop = apiResponse!.Result;

            if (coffeeshop == null)
            {
                return NotFound();
            }
            else
            {
                CoffeeShop = coffeeshop;
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            var result = await _apiShop.PutAsync($"{CoffeeShop.CoffeeShopId}", CoffeeShop);
            if (!result)
            {
                TempData["shop-msg"] = "Update shop success";
                return RedirectToPage("./Index");
            }
            TempData["shop-msg"] = "Update shop Fail";
            return Page();

        }

    }
}
