using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Model
{
    public class BookingProduct
    {
        public int Quantity { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalPrice { get; set; }

        public int BookingId { get; set; }
        public Booking? Booking { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
