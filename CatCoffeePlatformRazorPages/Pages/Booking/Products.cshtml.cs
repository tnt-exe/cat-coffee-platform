using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;
using DAO.Context;
using DTO.Common;
using DTO.CoffeeShopDTO;
using DTO.AreaDTO;
using DTO.TimeFrameDTO;

namespace CatCoffeePlatformRazorPages.Pages.Booking
{
    public class ProductsModel : PageModel
    {

        public ProductsModel()
        {
        }

        public CoffeeShopResponseDTO? CoffeeShop { get; set; }
        public AreaDto? Area { get; set; }
        public TimeFrameDto? TimeFrame { get; set; }

        [BindProperty]
        public string? CoffeeShopJson { get; set; }
        [BindProperty]
        public string? AreaJson { get; set; }
        [BindProperty]
        public int? TimeFrameId { get; set; }
        [BindProperty]
        [ModelBinder(BinderType = typeof(DateOnlyModelBinder))]
        public DateOnly? BookedDate { get; set; }
        [BindProperty]
        public int? BookedSlots { get; set; } = 0;

        public IList<Product> Product { get;set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            return Page();
        }
    }
}
