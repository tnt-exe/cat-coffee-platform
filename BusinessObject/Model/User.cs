using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Model
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = default!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = default!;

        public byte? Role { get; set; }
        public byte? Status { get; set; }

        [Column(TypeName = "money")]
        public decimal Balance { get; set; }

        public bool Deleted { get; set; } = false;

        public int? CoffeeShopId { get; set; }
        public CoffeeShop? CoffeeShop { get; set; }

        public int? ManagerShopId { get; set; }
        public CoffeeShop? ManagedCoffeeShop { get; set; }

        public IEnumerable<Wallet> Wallets { get; set; } = new List<Wallet>();

        public IEnumerable<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
