using System.ComponentModel.DataAnnotations;

namespace DTO.CatDTO
{
    public record CatCreate
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string? CatName { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public int HealthyStatus { get; set; }

        [Required]
        public int AreaId { get; set; }

        public int? CoffeeShopId { get; set; }
    }
}
