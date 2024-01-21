using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Model
{
    public class Bill
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BillId { get; set; }

        public DateTime PaymentDate { get; set; }

        [Column(TypeName = "money")]
        public Decimal TotalMoney { get; set; }

        public int BookingId { get; set; }
        public Booking? Booking { get; set; }
    }
}
