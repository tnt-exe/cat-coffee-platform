using Newtonsoft.Json;

namespace DTO.Common.CustomJsonConverter
{
    internal class TimeOnlyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeOnly);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is TimeOnly))
            {
                throw new JsonSerializationException("Expected TimeOnly object value.");
            }

            var time = (TimeOnly?)value;
            writer.WriteValue(time?.ToString("HH:mm:ss"));
        }
    }
}
