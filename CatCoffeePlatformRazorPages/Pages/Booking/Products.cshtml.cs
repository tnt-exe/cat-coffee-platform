using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;
using DAO.Context;

namespace CatCoffeePlatformRazorPages.Pages.Booking
{
    public class ProductsModel : PageModel
    {
        private readonly DAO.Context.ApplicationDbContext _context;

        public ProductsModel(DAO.Context.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {
                Product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.CoffeeShop).ToListAsync();
            }
        }
    }
}
