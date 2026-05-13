using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sharpmine.Server.Core.Security.Models;

public class MinecraftDateConverter : JsonConverter<DateTimeOffset>
{

    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? dateString = reader.GetString();

        if (string.IsNullOrWhiteSpace(dateString) || dateString.Equals("forever", StringComparison.OrdinalIgnoreCase))
        {
            return DateTimeOffset.MaxValue;
        }

        bool couldParse = DateTimeOffset.TryParseExact(
            dateString,
            "yyyy-MM-dd HH:mm:ss K",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AllowWhiteSpaces,
            out var date);

        return (couldParse) ? date : DateTimeOffset.MaxValue;
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteStringValue((value == DateTimeOffset.MaxValue)
            ? "forever"
            : TrimTimeZoneColon(value.ToString("yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture)));
    }

    private static string TrimTimeZoneColon(string value) => value.Remove(value.Length - 3, 1);

}
