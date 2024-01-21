using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Model
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public int CoffeeShopId { get; set; }
        public CoffeeShop? CoffeeShop { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public IEnumerable<BookingProduct> BookingProducts { get; set; } = new List<BookingProduct>();
    }
}
