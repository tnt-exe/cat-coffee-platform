using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Model
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; }

        public byte? Role { get; set; }
        public byte? Status { get; set; }

        [Required]
        [Phone]
        public string? PhoneNumber { get; set; }

        public int? CoffeeShopId { get; set; }
        public CoffeeShop? CoffeeShop { get; set; }

        public int? ManagerShopId { get; set; }
        public CoffeeShop? ManagedCoffeeShop { get; set; }

        public IEnumerable<Payment> Payments { get; set; } = new List<Payment>();
    }
}
