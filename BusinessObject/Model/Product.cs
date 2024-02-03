using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        [Required]
        public int Quantity { get; set; }
        
        public string Unit { get; set; }
        public int CoffeeShopId { get; set; }
        public CoffeeShop? CoffeeShop { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [JsonIgnore]
        public IEnumerable<BookingProduct> BookingProducts { get; set; } = new List<BookingProduct>();
    }
}
