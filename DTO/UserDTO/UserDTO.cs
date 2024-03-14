using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DTO.UserDTO
{
    public class UserDTO
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        [MinLength(5)]
        public string? OldPassword { get; set; }
        [MinLength(5)]
        public string? Password { get; set; }
        [MinLength(5)]
        public string? ConfirmPassword { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? LockoutEnabled { get; set; } = false;
        public DateTimeOffset? LockoutEnd { get; set; }
        public byte? Role { get; set; }
        public byte? Status { get; set; }
        [EmailAddress]
        [AllowNull]
        public string? Email { get; set; }

        public int? CoffeeShopId { get; set; }
        public int? ManagerShopId { get; set; }
    }
}
