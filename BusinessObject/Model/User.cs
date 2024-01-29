using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Model
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; }

        public byte? Role { get; set; }
        public byte? Status { get; set; }

        public int? CoffeeShopId { get; set; }
        public CoffeeShop? CoffeeShop { get; set; }

        public int? ManagerShopId { get; set; }
        public CoffeeShop? ManagedCoffeeShop { get; set; }

        public IEnumerable<Wallet> Wallets { get; set; } = new List<Wallet>();

        public IEnumerable<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
