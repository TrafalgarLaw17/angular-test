using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class JsonTimeConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateString = reader.GetString();
        if (string.IsNullOrEmpty(dateString))
            return null;

        return DateTime.Parse(dateString).ToUniversalTime(); 
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            writer.WriteStringValue(value.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")); 
        else
            writer.WriteNullValue();
    }
}