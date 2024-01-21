using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Model
{
    public class Payment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }

        [Required]
        public string? AccountName { get; set; }

        [Required]
        public string? PaymentType { get; set; }
        public string? CardType { get; set; }
        public string? BankName { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
