using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;
using DAO.Context;

namespace CatCoffeePlatformRazorPages.Pages.CoffeeShopPages
{
    public class EditModel : PageModel
    {
        private readonly DAO.Context.ApplicationDbContext _context;

        public EditModel(DAO.Context.ApplicationDbContext context)
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

            var coffeeshop =  await _context.CoffeeShops.FirstOrDefaultAsync(m => m.CoffeeShopId == id);
            if (coffeeshop == null)
            {
                return NotFound();
            }
            CoffeeShop = coffeeshop;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(CoffeeShop).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoffeeShopExists(CoffeeShop.CoffeeShopId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CoffeeShopExists(int id)
        {
          return (_context.CoffeeShops?.Any(e => e.CoffeeShopId == id)).GetValueOrDefault();
        }
    }
}
