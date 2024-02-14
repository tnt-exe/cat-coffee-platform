using DTO.Helper;
using System.ComponentModel.DataAnnotations;

namespace DTO.TimeFrameDTO
{
    public record TimeFrameCreate
    {
        [Required]
        [TimeValidator]
        public string? StartTime { get; set; }

        [Required]
        [TimeValidator]
        public string? EndTime { get; set; }

        [Required]
        [Range(1, 9999, ErrorMessage = "Invalid Price")]
        public decimal Price { get; set; }

        public int CoffeeShopId { get; set; }
    }
}
