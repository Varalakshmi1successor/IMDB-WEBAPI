using System.Text.Json.Serialization;
using System.Text.Json;
using IMDBLite.API.Models.DB;

public class GenderEnumConverter : JsonConverter<Gender?>
{
    public override Gender? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (string.IsNullOrEmpty(value))
            return null; 

        if (Enum.TryParse<Gender>(value, ignoreCase: true, out var gender))
        {
            return gender;
        }

        throw new JsonException($"Invalid gender '{value}' provided. Allowed values: {string.Join(", ", Enum.GetNames(typeof(Gender)))}");
    }

    public override void Write(Utf8JsonWriter writer, Gender? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString());
    }
}
