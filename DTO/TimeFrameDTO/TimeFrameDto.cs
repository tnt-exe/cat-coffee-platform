namespace DTO.TimeFrameDTO
{
    public record TimeFrameDto
    {
        public int TimeFrameId { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }

        public decimal Price { get; set; }

        public int CoffeeShopId { get; set; }
        public string? CoffeeShop { get; set; }
    }
}
