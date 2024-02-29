using System.Text.Json;
using System.Text.Json.Serialization;
namespace CatCoffeePlatformRazorPages.Common;
public class TimeOnlyConverter : JsonConverter<TimeOnly>
{
    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            // Read the nested object
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                // Extract hour, minute, and second from the nested object
                var root = doc.RootElement;
                int hour = root.GetProperty("hour").GetInt32();
                int minute = root.GetProperty("minute").GetInt32();
                int second = root.GetProperty("second").GetInt32();

                return new TimeOnly(hour, minute, second);
            }
        }

        throw new JsonException($"Unable to parse TimeOnly from: {reader.GetString()}");
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("hour", value.Hour);
        writer.WriteNumber("minute", value.Minute);
        writer.WriteNumber("second", value.Second);
        writer.WriteEndObject();
    }
}
