using DTO.CoffeeShopDTO;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages
{
    public class HomeModel : PageModel
    {
        public HomeModel()
        {
        }

        public IEnumerable<CoffeeShopResponseDTO> CoffeeShops { get; set; } = default!;
    }
}