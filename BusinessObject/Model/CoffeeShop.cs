using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Model
{
    public class CoffeeShop
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CoffeeShopId { get; set; }

        [Required]
        public string? ShopName { get; set; }

        public string? Address { get; set; }
        public TimeOnly OpeningTime { get; set; }
        public TimeOnly ClosingTime { get; set; }

        [Required]
        public string ContactNumber { get; set; } = "00-0000-000";

        [Required]
        public string? Email { get; set; }
        public string? Description { get; set; }

        public bool Deleted { get; set; } = false;
        public User? Manager { get; set; }
        public Guid ManagerId { get; set; }

        public IEnumerable<User> Staffs { get; set; } = new List<User>();

        public IEnumerable<Booking> Bookings { get; set; } = new List<Booking>();

        public IEnumerable<Area> Areas { get; set; } = new List<Area>();

        public IEnumerable<Product> Products { get; set; } = new List<Product>();

        public IEnumerable<Cat> Cats { get; set; } = new List<Cat>();

        public IEnumerable<TimeFrame> TimeFrames { get; set; } = new List<TimeFrame>();
    }
}
