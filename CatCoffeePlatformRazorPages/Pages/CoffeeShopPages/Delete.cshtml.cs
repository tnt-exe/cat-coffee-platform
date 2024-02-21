using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;
using DAO.Context;

namespace CatCoffeePlatformRazorPages.Pages.CoffeeShopPages
{
    public class DeleteModel : PageModel
    {
        private readonly DAO.Context.ApplicationDbContext _context;

        public DeleteModel(DAO.Context.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public CoffeeShop CoffeeShop { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.CoffeeShops == null)
            {
                return NotFound();
            }

            var coffeeshop = await _context.CoffeeShops.FirstOrDefaultAsync(m => m.CoffeeShopId == id);

            if (coffeeshop == null)
            {
                return NotFound();
            }
            else 
            {
                CoffeeShop = coffeeshop;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.CoffeeShops == null)
            {
                return NotFound();
            }
            var coffeeshop = await _context.CoffeeShops.FindAsync(id);

            if (coffeeshop != null)
            {
                CoffeeShop = coffeeshop;
                _context.CoffeeShops.Remove(CoffeeShop);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
