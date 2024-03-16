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
using DTO.ProductDTO;
using Google.Protobuf.WellKnownTypes;
using DTO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CatCoffeePlatformRazorPages.Pages.ProductPages
{
    public class CreateModel : PageModel
    {
        private readonly ApiHelper _apiProduct;
        private readonly ApiHelper _apiCategory;

        public CreateModel()
        {
            _apiProduct = new ApiHelper(ApiResources.Products);
            _apiCategory = new ApiHelper(ApiResources.Categories);
        }

        public async Task<IActionResult> OnGetAsync(int? shopId)
        {
            if (shopId == null)
            {
                return NotFound();
            }

            ViewData["shopId"] = shopId;
            
            await _loadSelectList();
            return Page();
        }

        [BindProperty]
        public ProductCreate Product { get; set; } = default!;

        [BindProperty]
        public int ShopId { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                await _loadSelectList();
                return Page();
            }

            bool result = await _apiProduct.PostAsync(Product);

            if (!result)
            {
                await _loadSelectList();
                return Page();
            }

            TempData["product-msg"] = "Product created successfully";

            return Redirect("/CoffeeShopPages/Details?id=" + ShopId);
        }

        private async Task _loadSelectList()
        {
            var apiResponse = await _apiCategory.GetAsync<ResponseBody<IEnumerable<CategoryDto>>>();
            var categoryList = apiResponse!.Result;

            if (categoryList is not null)
            {
                ViewData["CategoryId"] = new SelectList(categoryList, "CategoryId", "CategoryName");
            }
        }
    }
}
