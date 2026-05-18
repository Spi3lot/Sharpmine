using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sharpmine.Server.Infrastructure.Security.Models;

public class HyphenlessGuidConverter : JsonConverter<Guid>
{

    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? raw = reader.GetString();
        return (raw is not null) ? Guid.Parse(raw) : Guid.Empty;
    }

    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("N"));
    }

}
