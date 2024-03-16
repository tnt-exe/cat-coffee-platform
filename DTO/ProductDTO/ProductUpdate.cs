using System.ComponentModel.DataAnnotations;

namespace DTO.ProductDTO;

public record ProductUpdate
{
    [Required]
    public string? ProductName { get; set; }

    [Required]
    public decimal? Price { get; set; }

    [Required]
    public int? Quantity { get; set; }

    [Required]
    public string? Unit { get; set; }

    [Required]
    public int? CategoryId { get; set; }
}