using System.Text.Json;
using System.Text.Json.Serialization;

using Sharpmine.Server.Protocol.DataTypes;

namespace Sharpmine.Server.Security.Models;

public class UuidConverter : JsonConverter<Uuid>
{

    public override Uuid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? raw = reader.GetString();
        return (raw is not null) ? Uuid.Parse(raw) : Uuid.Empty;
    }

    public override void Write(Utf8JsonWriter writer, Uuid value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(((Guid) value).ToString("N"));
    }

}
