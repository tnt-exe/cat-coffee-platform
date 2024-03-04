using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Model;
using DAO.Context;
using DTO.CoffeeShopDTO;

namespace CatCoffeePlatformRazorPages.Pages.CoffeeShop
{
    public class CreateModel : PageModel
    {
        private readonly DAO.Context.ApplicationDbContext _context;

        public CreateModel(DAO.Context.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CoffeeShopCreate CoffeeShop { get; set; } = default!;
        
    }
}
