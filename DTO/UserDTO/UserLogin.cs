using System.ComponentModel.DataAnnotations;

namespace DTO.UserDTO
{
    public class UserLogin
    {
        [Required]
        public string UserName { get; set; } = default!;

        [Required]
        [MinLength(5)]
        public string Password { get; set; } = default!;
    }
}
