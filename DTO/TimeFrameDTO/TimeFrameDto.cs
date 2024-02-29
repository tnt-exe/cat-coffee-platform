using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace DTO.TimeFrameDTO
{
    public record TimeFrameDto
    {
        [Key]
        public int TimeFrameId { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }

        public decimal Price { get; set; }

        public int CoffeeShopId { get; set; }
        public string? CoffeeShop { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
