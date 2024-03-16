using BusinessObject.Model;
using CatCoffeePlatformRazorPages.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages.ProductPages
{
    public class IndexModel : PageModel
    {
        private readonly ApiHelper _apiProduct;

        public IndexModel()
        {
            _apiProduct = new ApiHelper(ApiResources.Products);
        }

        public IEnumerable<Product> Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? shopId)
        {
            if (shopId == null)
            {
                return NotFound();
            }

            IEnumerable<Product>? productList = await _apiProduct
                .GetQueryAsync<IEnumerable<Product>>($"&shopId={shopId}&includeProperties=Category");

            if (productList == null)
            {
                return NotFound();
            }

            productList = productList
                .Where(x => x.CoffeeShopId == shopId);

            Product = productList;

            ViewData["shopId"] = shopId;

            return Page();
        }
    }
}
