using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sharpmine.Server.Security.Models;

public class MinecraftDateConverter : JsonConverter<DateTimeOffset?>
{

    private const string Format = "yyyy-MM-dd HH:mm:ss Z";

    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? dateString = reader.GetString();

        if (string.IsNullOrWhiteSpace(dateString) || dateString.Equals("forever", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        bool couldParse = DateTimeOffset.TryParseExact(
            dateString,
            Format,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var date);

        return (couldParse) ? date : null;
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString(Format) ?? "forever");
    }

}
