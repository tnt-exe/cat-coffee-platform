using BusinessObject.Model;
using CatCoffeePlatformRazorPages.Common;
using DTO;
using DTO.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CatCoffeePlatformRazorPages.Pages.ProductPages
{
    public class EditModel : PageModel
    {
        private readonly ApiHelper _apiProduct;
        private readonly ApiHelper _apiCategory;

        public EditModel()
        {
            _apiProduct = new ApiHelper(ApiResources.Products);
            _apiCategory = new ApiHelper(ApiResources.Categories);
        }

        [BindProperty]
        public ProductUpdate Product { get; set; } = default!;

        [BindProperty]
        public int ShopId { get; set; }

        [BindProperty]
        public int ProductId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? productId, int? shopId)
        {
            if (productId == null || shopId == null)
            {
                return NotFound();
            }

            IEnumerable<Product>? productRes = await _apiProduct
                .GetQueryAsync<IEnumerable<Product>>($"productId={productId}&shopId={shopId}&includeProperties=Category");

            if (productRes == null || !productRes.Any())
            {
                return NotFound();
            }

            var product = productRes.FirstOrDefault(p => p.ProductId == productId)!;

            Product = new ProductUpdate
            {
                ProductName = product.ProductName,
                Price = product.Price,
                Quantity = product.Quantity,
                CategoryId = product.CategoryId,
                Unit = product.Unit,
            };

            await _loadCategoryList();

            ViewData["shopId"] = shopId;
            ViewData["productId"] = productId;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await _loadCategoryList();
                return Page();
            }

            bool result = await _apiProduct.PutQueryAsync($"productId={ProductId}&shopId={ShopId}", Product);

            if (!result)
            {
                await _loadCategoryList();
                return Page();
            }

            TempData["product-msg"] = "Update product success";

            return Redirect("/CoffeeShopPages/Details?id=" + ShopId);
        }

        private async Task _loadCategoryList()
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
