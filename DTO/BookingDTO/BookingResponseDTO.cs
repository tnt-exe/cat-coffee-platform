using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.BookingDTO
{
    public class BookingResponseDTO
    {
        public int? BookingId { get; set; }
        public DateTime? BookingDate { get; set; }
        public string? Date { get; set; }
        public int? Slots { get; set; }
        public Decimal? TotalMoney { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? Status { get; set; }
        public int? AreaId { get; set; }
        public int? TimeFrameId { get; set; }
        public Guid? UserId { get; set; }
        public int? CoffeeShopId { get; set; }
    }
}
