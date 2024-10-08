﻿using CatCoffeePlatformRazorPages.Common;
using DTO.CoffeeShopDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace CatCoffeePlatformRazorPages.Pages.Booking
{
    public class CoffeeShopsModel : PageModel
    {
        private readonly ApiHelper _apiCoffeeShop;
        private IHttpContextAccessor httpContextAccessor;

        public CoffeeShopsModel(IHttpContextAccessor httpContextAccessor)
        {
            _apiCoffeeShop = new ApiHelper(ApiResources.CoffeeShops);
            this.httpContextAccessor = httpContextAccessor;
        }

        public IList<CoffeeShopResponseDTO> CoffeeShops { get; set; } = default!;

        [BindProperty]
        public string? SearchValue { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var isAuthenticated = httpContextAccessor.HttpContext?.User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier) ?? false;
            if (!isAuthenticated)
            {
                return RedirectToPage("../login");
            }

            var apiResponse = await _apiCoffeeShop
                .GetAsync<ResponseBody<IEnumerable<CoffeeShopResponseDTO>>>();
            CoffeeShops = apiResponse?.Result?.ToArray() ?? new CoffeeShopResponseDTO[0];
            return Page();
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            var isAuthenticated = httpContextAccessor.HttpContext?.User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier) ?? false;
            if (!isAuthenticated)
            {
                return RedirectToPage("../login");
            }

            var apiResponse = await _apiCoffeeShop
                .GetAsync<ResponseBody<IEnumerable<CoffeeShopResponseDTO>>>();
            var coffeeShops = apiResponse?.Result?.ToList();
            if (SearchValue is not null)
            {
                var coffeeShopList = coffeeShops?.Where(c => (c.ShopName?.Contains(SearchValue) ?? false)
                || (c.Address?.Contains(SearchValue) ?? false)
                || (c.ContactNumber?.Contains(SearchValue) ?? false)
                || (c.Email?.Contains(SearchValue) ?? false)
                || (c.Description?.Contains(SearchValue) ?? false))?.ToArray() ?? new CoffeeShopResponseDTO[0];
                CoffeeShops = coffeeShopList;
            }
            else
            {
                CoffeeShops = coffeeShops?.ToArray() ?? new CoffeeShopResponseDTO[0];
            }
            return Page();
        }
    }
}
