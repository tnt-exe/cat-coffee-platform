namespace DTO.CatDTO
{
    public record CatDto
    {
        public int CatId { get; set; }
        public string? CatName { get; set; }

        public string? Description { get; set; }
        public int HealthyStatus { get; set; }

        public int AreaId { get; set; }
        public string? Area { get; set; }

        public int? CoffeeShopId { get; set; }
        public string? CoffeeShop { get; set; }
    }
}
