using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DTO.AreaDTO
{
    public record AreaDto
    {
        [Key]
        public int AreaId { get; set; }
        public string? AreaName { get; set; }

        public string? Description { get; set; }
        public decimal PricePerHour { get; set; }
        public int MaxSlots { get; set; }

        public int CoffeeShopId { get; set; }
        public string? CoffeeShop { get; set; }

        [JsonIgnore]
        public int AvailableSlots { get; set; } = 0;

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
