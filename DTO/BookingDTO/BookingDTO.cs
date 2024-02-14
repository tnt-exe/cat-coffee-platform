using DTO.Common;
using Microsoft.AspNetCore.Mvc;

namespace DTO.BookingDTO;

[ModelBinder(BinderType = typeof(BookingDTOModelBinder))]
public class BookingDTO
{
    public DateOnly? Date { get; set; }
    public int? Slots { get; set; }
    public Decimal? TotalMoney { get; set; }
    public int? Status { get; set; }
    public int? AreaId { get; set; }
    public int? TimeFrameId { get; set; }
    public Guid? UserId { get; set; }
    public int? CoffeeShopId { get; set; }
    public IEnumerable<BookingDTO_BookingProduct> BookingProducts { get; set; } = new List<BookingDTO_BookingProduct>();
}

public class BookingDTO_BookingProduct
{
    public int? Quantity { get; set; }
    public decimal? TotalPrice { get; set; }
    public int? ProductId { get; set; }
}
