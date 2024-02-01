namespace DTO.CatDTO
{
    public class CatDto : CatCreate
    {
        public int CatId { get; set; }
        public string? Area { get; set; }
        public string? CoffeeShop { get; set; }
    }
}
