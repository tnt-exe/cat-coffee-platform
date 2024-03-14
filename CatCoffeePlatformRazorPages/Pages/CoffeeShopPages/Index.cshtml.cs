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
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CatCoffeePlatformRazorPages.Pages.CoffeeShopPages
{
    public class IndexModel : PageModel
    {
        private readonly ApiHelper _apiShop;
        private IHttpContextAccessor httpContextAccessor;
        public IndexModel(IHttpContextAccessor httpContextAccessor)
        {
            _apiShop = new ApiHelper(ApiResources.CoffeeShops);
            this.httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<CoffeeShopResponseDTO> CoffeeShop { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var isAuthenticated = httpContextAccessor.HttpContext?.User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier) ?? false;
            if (!isAuthenticated)
            {
                return RedirectToPage("../login");
            }
            var apiResponse = await _apiShop.GetAsync<ResponseBody<IEnumerable<CoffeeShopResponseDTO>>>();
            var shopList = apiResponse!.Result;
            if (shopList is not null)
            {
                CoffeeShop = shopList;
                return Page();
            }
            return NotFound();
        }
    }
}
