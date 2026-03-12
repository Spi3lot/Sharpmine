using System.Text.Json;
using System.Text.Json.Serialization;
using Sharpmine.Server.Protocol.Models;

namespace Sharpmine.Server.Protocol.Converters;

public class TextComponentConverter : JsonConverter<TextComponent>
{

    public override TextComponent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                return TextComponent.Literal(reader.GetString()!);
            case JsonTokenType.StartArray:
            {
                var list = JsonSerializer.Deserialize<List<TextComponent>>(ref reader, options);

                if (list is null or { Count: 0 })
                {
                    return new TextComponent();
                }

                var root = list[0];

                if (list.Count == 1)
                {
                    return root;
                }

                var extra = root.Extra ?? [];
                return root with { Extra = [..extra, ..list[1..]] };
            }
            case JsonTokenType.None:
            case JsonTokenType.StartObject:
            case JsonTokenType.EndObject:
            case JsonTokenType.EndArray:
            case JsonTokenType.PropertyName:
            case JsonTokenType.Comment:
            case JsonTokenType.Number:
            case JsonTokenType.True:
            case JsonTokenType.False:
            case JsonTokenType.Null:
            default:
                return JsonSerializer.Deserialize<TextComponent>(ref reader, options)!;
        }
    }

    public override void Write(Utf8JsonWriter writer, TextComponent value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case { IsSimple: false }:
                JsonSerializer.Serialize(writer, value, options);
                return;
            case { Extra: not null }:
            {
                writer.WriteStartArray();
                JsonSerializer.Serialize(writer, value with { Extra = null }, options);

                foreach (var extra in value.Extra)
                {
                    JsonSerializer.Serialize(writer, extra, options);
                }

                writer.WriteEndArray();
                return;
            }
            case { Text: not null }:
                writer.WriteStringValue(value.Text);
                return;
            default:
                throw new InvalidOperationException($"{nameof(value)} seems to have nothing to write: {value}");
        }
    }

}