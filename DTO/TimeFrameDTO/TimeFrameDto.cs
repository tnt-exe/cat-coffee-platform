namespace DTO.TimeFrameDTO
{
    public record TimeFrameDto
    {
        public int TimeFrameId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public decimal Price { get; set; }

        public int CoffeeShopId { get; set; }
        public string? CoffeeShop { get; set; }
    }
}
