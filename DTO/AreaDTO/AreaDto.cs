namespace DTO.AreaDTO
{
    public record AreaDto
    {
        public int AreaId { get; set; }
        public string? AreaName { get; set; }

        public string? Description { get; set; }
        public decimal PricePerHour { get; set; }
        public int MaxSlots { get; set; }

        public int CoffeeShopId { get; set; }
        public string? CoffeeShop { get; set; }
    }
}
