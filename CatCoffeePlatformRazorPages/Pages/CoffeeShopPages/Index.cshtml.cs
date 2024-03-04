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
    public class IndexModel : PageModel
    {
        private readonly ApiHelper _apiShop;

        public IndexModel()
        {
            _apiShop = new ApiHelper(ApiResources.CoffeeShops);
        }

        public IEnumerable<CoffeeShopResponseDTO> CoffeeShop { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var apiResponse = await _apiShop.GetAsync<ResponseBody<IEnumerable<CoffeeShopResponseDTO>>>();
            var shopList = apiResponse!.Result;
            if (shopList is not null)
            {
                CoffeeShop = shopList;
            }
        }
    }
}
