using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace DTO.CoffeeShopDTO
{
    public class CoffeeShopResponseDTO
    {
        public int CoffeeShopId { get; set; }
        [Required]
        public string? ShopName { get; set; }
        public string? Address { get; set; }
        public string? OpeningTime { get; set; }
        public string? ClosingTime { get; set; }

        [Required]
        public string ContactNumber { get; set; } = "00-0000-000";

        [Required]
        public string? Email { get; set; }
        public string? Description { get; set; }

        public bool Deleted { get; set; }
        public Guid ManagerId { get; set; }
        public string? ManagerEmail { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
