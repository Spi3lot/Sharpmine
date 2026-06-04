using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sharpmine.Domain;

public class IdentifierConverter : JsonConverter<Identifier>
{

    public override Identifier Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();

        return (string.IsNullOrWhiteSpace(value))
            ? throw new JsonException("Identifier string cannot be null or whitespace.")
            : value;
    }

    public override void Write(Utf8JsonWriter writer, Identifier value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }

}
