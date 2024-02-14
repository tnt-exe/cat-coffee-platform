using System.ComponentModel.DataAnnotations;

namespace DTO.AreaDTO
{
    public record AreaCreate
    {
        [Required]
        [MaxLength(50)]
        public string? AreaName { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [Required]
        [Range(1, 9999, ErrorMessage = "Invalid Price")]
        public decimal PricePerHour { get; set; }

        [Required]
        [Range(1, 2024, ErrorMessage = "Invalid Slot number")]
        public int MaxSlots { get; set; }

        public int CoffeeShopId { get; set; }
    }
}
