using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;
using DAO.Context;
using CatCoffeePlatformRazorPages.Common;
using DTO.CoffeeShopDTO;

namespace CatCoffeePlatformRazorPages.Pages.CoffeeShopPages
{
    public class DeleteModel : PageModel
    {
        private readonly ApiHelper _apiShop;

        public DeleteModel()
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var result = await _apiShop.DeleteAsync(id.Value);
            if (!result)
            {
                TempData["shop-msg"] = "Delete shop success";
                return RedirectToPage("./Index");
            }
            TempData["shop-msg"] = "Delete shop Fail";
            return Page();
        }
    }
}
