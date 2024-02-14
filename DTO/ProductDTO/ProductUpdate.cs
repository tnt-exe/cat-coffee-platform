namespace DTO.ProductDTO;

public record ProductUpdate
{
    public string? ProductName { get; set; }
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public string? Unit { get; set; }
    public int? CategoryId { get; set; }
}