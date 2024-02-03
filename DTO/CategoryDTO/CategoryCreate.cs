using System.ComponentModel.DataAnnotations;

namespace DTO.CategoryDTO
{
    public record CategoryUpsert
    {
        [Required]
        public string CategoryName { get; init; }
    }
}
