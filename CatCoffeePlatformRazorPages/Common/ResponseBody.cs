namespace CatCoffeePlatformRazorPages.Common
{
    public class ResponseBody<T> where T : class
    {
        public string? Title { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public string? Token { get; set; }
        public T? Result { get; set; }
    }
}
