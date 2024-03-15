using DTO.Common.CustomJsonConverter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.BookingDTO
{
    //[JsonConverter(typeof(BookingResponseDTOConverter))]
    public class BookingResponseDTO
    {
        [Key]
        public int? BookingId { get; set; }
        public DateTime? BookingDate { get; set; }
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateOnly Date { get; set; }
        public int? Slots { get; set; }
        public Decimal? TotalMoney { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? PaymentStatus { get; set; }
        public int? Status { get; set; }
        public bool Deleted { get; set; }
        public int? AreaId { get; set; }
        public BookingResponseDTO_Area? Area { get; set; }
        public int? TimeFrameId { get; set; }
        public BookingResponseDTO_TimeFrame? TimeFrame { get; set; }
        public Guid? UserId { get; set; }
        public BookingResponseDTO_User? User { get; set; }
        public int? CoffeeShopId { get; set; }
        public BookingResponseDTO_CoffeeShop? CoffeeShop { get; set; }
        public IEnumerable<BookingResponseDTO_Product> Products { get; set; } = new List<BookingResponseDTO_Product>();
    }

    public class BookingResponseDTO_CoffeeShop
    {
        public int? CoffeeShopId { get; set; }
        public string? ShopName { get; set; }
        public string? Address { get; set; }
        [JsonConverter(typeof(TimeOnlyConverter))]
        public TimeOnly? OpeningTime { get; set; }
        [JsonConverter(typeof(TimeOnlyConverter))]
        public TimeOnly? ClosingTime { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
    }

    public class BookingResponseDTO_Area
    {
        public int? AreaId { get; set; }
        public string? AreaName { get; set; }
        public string? Description { get; set; }
        public decimal? PricePerHour { get; set; }
        public int? MaxSlots { get; set; }
    }

    public class BookingResponseDTO_TimeFrame
    {
        public int? TimeFrameId { get; set; }
        [JsonConverter(typeof(TimeOnlyConverter))]
        public TimeOnly? StartTime { get; set; }
        [JsonConverter(typeof(TimeOnlyConverter))]
        public TimeOnly? EndTime { get; set; }
        public decimal? Price { get; set; }
    }

    public class BookingResponseDTO_User
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class BookingResponseDTO_Product
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public string? Unit { get; set; }
    }
}
