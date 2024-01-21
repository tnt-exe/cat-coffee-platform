using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Model
{
    public class Cat
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CatId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? CatName { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
        public int HealthyStatus { get; set; }

        public int AreaId { get; set; }
        public Area? Area { get; set; }

        public int CoffeeShopId { get; set; }
        public CoffeeShop? CoffeeShop { get; set; }
    }
}
