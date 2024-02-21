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
using DTO.CatDTO;
using DTO.CoffeeShopDTO;

namespace CatCoffeePlatformRazorPages.Pages.CoffeeShopPages
{
    public class DetailsModel : PageModel
    {
        private readonly ApiHelper _apiShop;

        public DetailsModel()
        {
            _apiShop = new ApiHelper(ApiResources.CoffeeShops);
        }

        public CoffeeShopResponseDTO CoffeeShop { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var apiResponse = await _apiShop.GetAsync<ResponseBody<CoffeeShopResponseDTO>>($"{id}");
            var shop = apiResponse!.Result;
            if (shop == null)
            {
                return NotFound();
            }
            else
            {
                CoffeeShop = shop;
            }
            return Page();
        }
    }
}
