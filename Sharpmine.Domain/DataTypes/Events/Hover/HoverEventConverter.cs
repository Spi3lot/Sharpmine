using System.Text.Json;
using System.Text.Json.Serialization;

using Sharpmine.Domain.DataTypes.Components;

namespace Sharpmine.Domain.DataTypes.Events.Hover;

public class HoverEventConverter : JsonConverter<HoverEvent>
{

    public override HoverEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var root = JsonElement.ParseValue(ref reader);
        string action = root.GetProperty("action").GetString()!;
        var contents = root.GetProperty("contents");

        IHoverEventContents parsedContents = action switch
        {
            "show_text" => new ShowTextHoverEventContents(contents.Deserialize<Component>(options)!),
            "show_item" => new ShowItemHoverEventContents(
                contents.GetProperty("id").GetString()!,
                contents.TryGetProperty("count", out var c) ? c.GetInt32() : null,
                contents.TryGetProperty("tag", out var t) ? t.GetString() : null),
            "show_entity" => new ShowEntityHoverEventContents(
                contents.GetProperty("type").GetString()!,
                contents.GetProperty("id").GetString()!,
                contents.TryGetProperty("name", out var n) ? n.Deserialize<Component>(options) : null),
            _ => throw new JsonException($"Unknown hover event action: {action}")
        };

        return new HoverEvent(action, parsedContents);
    }

    public override void Write(Utf8JsonWriter writer, HoverEvent value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("action", value.Action);
        writer.WritePropertyName("contents");

        switch (value.Contents)
        {
            case ShowTextHoverEventContents text:
                JsonSerializer.Serialize(writer, text.Text, options);
                break;
            case ShowItemHoverEventContents item:
                writer.WriteStartObject();
                writer.WriteString("id", item.Id);
                if (item.Count is not null) writer.WriteNumber("count", item.Count.Value);
                if (item.Tag is not null) writer.WriteString("tag", item.Tag);
                writer.WriteEndObject();
                break;
            case ShowEntityHoverEventContents entity:
                writer.WriteStartObject();
                writer.WriteString("type", entity.Type);
                writer.WriteString("id", entity.Id);
                if (entity.Name is not null)
                {
                    writer.WritePropertyName("name");
                    JsonSerializer.Serialize(writer, entity.Name, options);
                }

                writer.WriteEndObject();
                break;
        }

        writer.WriteEndObject();
    }

}
