using Newtonsoft.Json;

namespace CatCoffeePlatformAPI.Common
{
    public class ResponseBody<T> where T : class
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Title { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string>? Errors { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Token { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T? Result { get; set; }
    }
}
