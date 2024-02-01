using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Model
{
    public class Booking
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public DateOnly Date { get; set; }
        public int Slots { get; set; }
        [Column(TypeName = "money")]
        public Decimal TotalMoney { get; set; }
        public DateTime PaymentDate { get; set; }
        public int Status { get; set; }

        public int AreaId { get; set; }
        public Area? Area { get; set; }

        public int TimeFrameId { get; set; }
        public TimeFrame? TimeFrame { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public int CoffeeShopId { get; set; }
        public CoffeeShop? CoffeeShop { get; set; }

        public IEnumerable<BookingProduct> BookingProducts { get; set; } = new List<BookingProduct>();

    }
}
