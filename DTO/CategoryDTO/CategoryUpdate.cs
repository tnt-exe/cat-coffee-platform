using System.ComponentModel.DataAnnotations;

namespace DTO;

public record CategoryUpdate
{
    [Required]
    public int CategoryId { get; set; }

    [Required]
    public string? CategoryName { get; set; }
}
