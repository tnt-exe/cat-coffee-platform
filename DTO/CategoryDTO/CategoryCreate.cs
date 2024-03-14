using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public record CategoryCreate
    {
        [Required]
        public string? CategoryName { get; init; }
    }
}
