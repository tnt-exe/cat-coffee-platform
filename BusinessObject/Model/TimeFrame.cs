using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Model
{
    public class TimeFrame
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TimeFrameId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public int CoffeeShopId { get; set; }
        public CoffeeShop? CoffeeShop { get; set; }
    }
}
