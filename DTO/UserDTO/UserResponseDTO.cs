namespace DTO.UserDTO
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public decimal? Balance { get; set; }
        public byte? Role { get; set; }
        public byte? Status { get; set; }
        public string? Email { get; set; }
        public int? CoffeeShopId { get; set; }
        public int? ManagerShopId { get; set; }
    }
}
