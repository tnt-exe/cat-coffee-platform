using DTO.Helper;
using System.ComponentModel.DataAnnotations;

namespace DTO.TimeFrameDTO
{
    public record TimeFrameUpdate
    {
        public int TimeFrameId { get; set; }

        [Required]
        [TimeValidator]
        public string? StartTime { get; set; }

        [Required]
        [TimeValidator]
        public string? EndTime { get; set; }

        [Required]
        [Range(1, 9999, ErrorMessage = "Invalid Price")]
        public decimal Price { get; set; }
    }
}
