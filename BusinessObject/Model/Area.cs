using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Model
{
    public class Area
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AreaId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? AreaName { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [Column(TypeName = "money")]
        public decimal PricePerHour { get; set; }

        [Required]
        [MinLength(1)]
        public int MaxSlots { get; set; }

        public int CoffeeShopId { get; set; }
        public CoffeeShop? CoffeeShop { get; set; }
    }
}
