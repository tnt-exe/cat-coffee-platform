using BusinessObject.Model;
using CatCoffeePlatformRazorPages.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages.ProductPages
{
    public class DetailsModel : PageModel
    {
        private readonly ApiHelper _apiProduct;

        public DetailsModel()
        {
            _apiProduct = new ApiHelper(ApiResources.Products);
        }

        public Product Product { get; set; } = default!;

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

            Product = productRes.FirstOrDefault(p => p.ProductId == productId)!;
            ViewData["shopId"] = shopId;

            return Page();
        }
    }
}
