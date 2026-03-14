using System.Text.Json;
using System.Text.Json.Serialization;

using Sharpmine.Server.Protocol.Models;

namespace Sharpmine.Server.Protocol.Converters;

public class TextComponentConverter : JsonConverter<TextComponent>
{

    public override TextComponent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => TextComponent.Literal(reader.GetString()!),
            JsonTokenType.StartArray => ReadArray(ref reader, options),
            JsonTokenType.StartObject => ReadObject(ref reader, options),
            _ => throw new JsonException($"Unexpected token {reader.TokenType} for {typeToConvert}.")
        };
    }

    private static TextComponent ReadObject(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        TextComponent.ContentType? type = null;

        string? text = null,
            translate = null,
            fallback = null,
            color = null,
            font = null,
            insertion = null,
            selector = null,
            keybind = null,
            nbt = null;

        bool? bold = null,
            italic = null,
            underlined = null,
            strikethrough = null,
            obfuscated = null;

        int? shadowColor = null;
        Score? score = null;
        ClickEvent? click = null;
        HoverEvent? hover = null;
        List<TextComponent>? with = null, extra = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject) break;
            if (reader.TokenType != JsonTokenType.PropertyName) continue;

            string propertyName = reader.GetString()!;
            reader.Read(); // Move to value

            switch (propertyName)
            {
                case "type": type = Enum.Parse<TextComponent.ContentType>(reader.GetString()!, ignoreCase: true); break;
                case "text": text = reader.GetString(); break;
                case "translate": translate = reader.GetString(); break;
                case "fallback": fallback = reader.GetString(); break;
                case "selector": selector = reader.GetString(); break;
                case "keybind": keybind = reader.GetString(); break;
                case "nbt": nbt = reader.GetString(); break;
                case "color": color = reader.GetString(); break;
                case "font": font = reader.GetString(); break;
                case "bold": bold = reader.GetBoolean(); break;
                case "italic": italic = reader.GetBoolean(); break;
                case "underlined": underlined = reader.GetBoolean(); break;
                case "strikethrough": strikethrough = reader.GetBoolean(); break;
                case "obfuscated": obfuscated = reader.GetBoolean(); break;
                case "insertion": insertion = reader.GetString(); break;
                case "shadow_color": shadowColor = ReadShadowColor(ref reader); break;
                case "with": with = JsonSerializer.Deserialize<List<TextComponent>>(ref reader, options); break;
                case "score": score = JsonSerializer.Deserialize<Score>(ref reader, options); break;
                case "click_event": click = JsonSerializer.Deserialize<ClickEvent>(ref reader, options); break;
                case "hover_event": hover = JsonSerializer.Deserialize<HoverEvent>(ref reader, options); break;
                case "extra": extra = JsonSerializer.Deserialize<List<TextComponent>>(ref reader, options); break;
                default: throw new JsonException($"Unexpected property name {propertyName} for ${nameof(TextComponent)}.");
            }
        }

        return new TextComponent
        {
            Type = type,
            Text = text,
            Translate = translate,
            Fallback = fallback,
            With = with,
            Score = score,
            Selector = selector,
            Keybind = keybind,
            Nbt = nbt,
            Color = color,
            Font = font,
            Bold = bold,
            Italic = italic,
            Underlined = underlined,
            Strikethrough = strikethrough,
            Obfuscated = obfuscated,
            Insertion = insertion,
            ShadowColor = shadowColor,
            ClickEvent = click,
            HoverEvent = hover,
            Extra = extra
        };
    }

    private static int? ReadShadowColor(ref Utf8JsonReader reader)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number => reader.GetInt32(),
            JsonTokenType.StartArray => ReadRgba(ref reader),
            _ => null
        };
    }

    private static int ReadRgba(ref Utf8JsonReader reader)
    {
        Span<byte> rgba = stackalloc byte[4];

        for (int i = 0; i < 4; i++)
        {
            reader.Read();
            rgba[i] = Convert.ToByte(byte.MaxValue * Math.Clamp(reader.GetSingle(), 0, 1));
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray)
        {
            // If we get here, there are more than 4 elements OR the reader is in a corrupted state.
            // We THROW to prevent the reader from being/staying in such state.
            throw new JsonException("Shadow color array is either too long or ended abruptly.");
        }

        return (rgba[3] << 24) | (rgba[0] << 16) | (rgba[1] << 8) | rgba[2];
    }

    private static TextComponent ReadArray(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var list = JsonSerializer.Deserialize<List<TextComponent>>(ref reader, options);
        return TextComponent.List(list);
    }

#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
    public override void Write(Utf8JsonWriter writer, TextComponent value, JsonSerializerOptions options)
#pragma warning restore S3776
    {
        if (value.IsLiteral())
        {
            writer.WriteStringValue(value.AsLiteral());
            return;
        }

        if (value.IsList())
        {
            JsonSerializer.Serialize(writer, value.AsList(), options);
            return;
        }

        writer.WriteStartObject();

        // Content
        if (value.Type.HasValue) writer.WriteString("type", value.Type.Value.ToString().ToLower());
        if (value.Text is not null) writer.WriteString("text", value.Text);
        if (value.Translate is not null) writer.WriteString("translate", value.Translate);
        if (value.Fallback is not null) writer.WriteString("fallback", value.Fallback);
        if (value.Selector is not null) writer.WriteString("selector", value.Selector);
        if (value.Keybind is not null) writer.WriteString("keybind", value.Keybind);
        if (value.Nbt is not null) writer.WriteString("nbt", value.Nbt);

        // Formatting
        if (value.Color is not null) writer.WriteString("color", value.Color);
        if (value.Font is not null) writer.WriteString("font", value.Font);
        if (value.Bold.HasValue) writer.WriteBoolean("bold", value.Bold.Value);
        if (value.Italic.HasValue) writer.WriteBoolean("italic", value.Italic.Value);
        if (value.Underlined.HasValue) writer.WriteBoolean("underlined", value.Underlined.Value);
        if (value.Strikethrough.HasValue) writer.WriteBoolean("strikethrough", value.Strikethrough.Value);
        if (value.Obfuscated.HasValue) writer.WriteBoolean("obfuscated", value.Obfuscated.Value);
        if (value.Insertion is not null) writer.WriteString("insertion", value.Insertion);
        if (value.ShadowColor.HasValue) writer.WriteNumber("shadow_color", value.ShadowColor.Value);

        // Nested
        if (value.Score is not null)
        {
            writer.WritePropertyName("with");
            JsonSerializer.Serialize(writer, value.With, options);
        }

        if (value.With is not null)
        {
            writer.WritePropertyName("with");
            JsonSerializer.Serialize(writer, value.With, options);
        }

        if (value.ClickEvent is not null)
        {
            writer.WritePropertyName("click_event");
            JsonSerializer.Serialize(writer, value.ClickEvent, options);
        }

        if (value.HoverEvent is not null)
        {
            writer.WritePropertyName("hover_event");
            JsonSerializer.Serialize(writer, value.HoverEvent, options);
        }

        if (value.Extra is not null)
        {
            writer.WritePropertyName("extra");
            JsonSerializer.Serialize(writer, value.Extra, options);
        }

        writer.WriteEndObject();
    }

}