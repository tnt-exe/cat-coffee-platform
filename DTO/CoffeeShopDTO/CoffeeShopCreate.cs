using System.ComponentModel.DataAnnotations;

namespace DTO.CoffeeShopDTO
{
    public class CoffeeShopCreate
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
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string? Email { get; set; }
        public string? Description { get; set; }

        [Required]
        public Guid ManagerId { get; set; }
    }
}
