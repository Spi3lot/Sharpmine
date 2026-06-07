using System.Text.Json;
using System.Text.Json.Serialization;

using Sharpmine.Domain.DataTypes.Events.Click;
using Sharpmine.Domain.DataTypes.Events.Hover;

namespace Sharpmine.Domain.DataTypes.Components;

public class ComponentConverter : JsonConverter<Component>
{

    public override Component Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return new TextComponent(reader.GetString()!);
        }

        if (reader.TokenType == JsonTokenType.StartArray)
        {
            var list = JsonSerializer.Deserialize<List<Component>>(ref reader, options);
            return Component.List(list) ?? new TextComponent(string.Empty);
        }

        var root = JsonElement.ParseValue(ref reader);
        string? typeStr = GetStr(root, "type");

        var clickEvent = root.TryGetProperty("click_event", out var ce) ? ce.Deserialize<ClickEvent>(options) : null;
        var hoverEvent = root.TryGetProperty("hover_event", out var he) ? he.Deserialize<HoverEvent>(options) : null;
        var extra = root.TryGetProperty("extra", out var ex) ? ex.Deserialize<List<Component>>(options) : null;

        var style = new ComponentStyle(
            Color: GetStr(root, "color"),
            Font: GetStr(root, "font"),
            Bold: GetBool(root, "bold"),
            Italic: GetBool(root, "italic"),
            Underlined: GetBool(root, "underlined"),
            Strikethrough: GetBool(root, "strikethrough"),
            Obfuscated: GetBool(root, "obfuscated"),
            ShadowColor: ParseShadowColor(root),
            Insertion: GetStr(root, "insertion")
        );

        Component component = typeStr switch
        {
            "translatable" => new TranslatableComponent(
                root.GetProperty("translate").GetString()!,
                GetStr(root, "fallback"),
                root.TryGetProperty("with", out var w) ? w.Deserialize<List<Component>>(options) : null),
            "score" => new ScoreComponent(root.GetProperty("score").Deserialize<Score>(options)!),
            "selector" => new SelectorComponent(
                root.GetProperty("selector").GetString()!,
                root.TryGetProperty("separator", out var sep) ? sep.Deserialize<Component>(options) : null),
            "keybind" => new KeybindComponent(root.GetProperty("keybind").GetString()!),
            "nbt" => new NbtComponent(
                root.GetProperty("nbt").GetString()!,
                GetStr(root, "source"),
                GetBool(root, "interpret"),
                GetBool(root, "plain"),
                root.TryGetProperty("separator", out var sep) ? sep.Deserialize<Component>(options) : null,
                GetStr(root, "entity"),
                GetStr(root, "block"),
                GetStr(root, "storage")),
            _ when root.TryGetProperty("translate", out var tr) => new TranslatableComponent(
                tr.GetString()!,
                GetStr(root, "fallback"),
                root.TryGetProperty("with", out var w) ? w.Deserialize<List<Component>>(options) : null),
            _ when root.TryGetProperty("score", out var sc) => new ScoreComponent(sc.Deserialize<Score>(options)!),
            _ when root.TryGetProperty("selector", out var sel) => new SelectorComponent(
                sel.GetString()!,
                root.TryGetProperty("separator", out var sep) ? sep.Deserialize<Component>(options) : null),
            _ when root.TryGetProperty("keybind", out var kb) => new KeybindComponent(kb.GetString()!),
            _ when root.TryGetProperty("nbt", out var nbt) => new NbtComponent(
                nbt.GetString()!,
                GetStr(root, "source"),
                GetBool(root, "interpret"),
                GetBool(root, "plain"),
                root.TryGetProperty("separator", out var sep) ? sep.Deserialize<Component>(options) : null,
                GetStr(root, "entity"),
                GetStr(root, "block"),
                GetStr(root, "storage")),
            _ => new TextComponent(GetStr(root, "text") ?? string.Empty)
        };

        return component with
        {
            Style = style,
            ClickEvent = clickEvent,
            HoverEvent = hoverEvent,
            Extra = extra,
        };
    }

    public override void Write(Utf8JsonWriter writer, Component value, JsonSerializerOptions options)
    {
        if (value is TextComponent { IsPlain: true } plain)
        {
            writer.WriteStringValue(plain.Text);
            return;
        }

        if (value.IsList)
        {
            JsonSerializer.Serialize(writer, value.AsList(), options);
            return;
        }

        writer.WriteStartObject();

        switch (value)
        {
            case TextComponent l:
                writer.WriteString("text", l.Text);
                break;
            case TranslatableComponent t:
                writer.WriteString("type", "translatable");
                writer.WriteString("translate", t.Translate);
                if (t.Fallback is not null) writer.WriteString("fallback", t.Fallback);
                if (t.With is { Count: > 0 })
                {
                    writer.WritePropertyName("with");
                    JsonSerializer.Serialize(writer, t.With, options);
                }
                break;
            case ScoreComponent s:
                writer.WriteString("type", "score");
                writer.WritePropertyName("score");
                JsonSerializer.Serialize(writer, s.Score, options);
                break;
            case SelectorComponent sel:
                writer.WriteString("type", "selector");
                writer.WriteString("selector", sel.Selector);
                if (sel.Separator is not null)
                {
                    writer.WritePropertyName("separator");
                    JsonSerializer.Serialize(writer, sel.Separator, options);
                }
                break;
            case KeybindComponent k:
                writer.WriteString("type", "keybind");
                writer.WriteString("keybind", k.Keybind);
                break;
            case NbtComponent n:
                writer.WriteString("type", "nbt");
                writer.WriteString("nbt", n.Nbt);
                if (n.Source is not null) writer.WriteString("source", n.Source);
                if (n.Interpret is not null) writer.WriteBoolean("interpret", n.Interpret.Value);
                if (n.Plain is not null) writer.WriteBoolean("plain", n.Plain.Value);
                if (n.Entity is not null) writer.WriteString("entity", n.Entity);
                if (n.Block is not null) writer.WriteString("block", n.Block);
                if (n.Storage is not null) writer.WriteString("storage", n.Storage);
                if (n.Separator is not null)
                {
                    writer.WritePropertyName("separator");
                    JsonSerializer.Serialize(writer, n.Separator, options);
                }
                break;
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

        if (value.Extra is { Count: > 0 })
        {
            writer.WritePropertyName("extra");
            JsonSerializer.Serialize(writer, value.Extra, options);
        }

        if (value.Style.Color is not null) writer.WriteString("color", value.Style.Color);
        if (value.Style.Font is not null) writer.WriteString("font", value.Style.Font);
        if (value.Style.Bold is not null) writer.WriteBoolean("bold", value.Style.Bold.Value);
        if (value.Style.Italic is not null) writer.WriteBoolean("italic", value.Style.Italic.Value);
        if (value.Style.Underlined is not null) writer.WriteBoolean("underlined", value.Style.Underlined.Value);
        if (value.Style.Strikethrough is not null) writer.WriteBoolean("strikethrough", value.Style.Strikethrough.Value);
        if (value.Style.Obfuscated is not null) writer.WriteBoolean("obfuscated", value.Style.Obfuscated.Value);
        if (value.Style.ShadowColor is not null) writer.WriteNumber("shadow_color", value.Style.ShadowColor.Value);
        if (value.Style.Insertion is not null) writer.WriteString("insertion", value.Style.Insertion);

        writer.WriteEndObject();
    }

    private static string? GetStr(JsonElement root, string prop)
    {
        return root.TryGetProperty(prop, out var el) ? el.GetString() : null;
    }

    private static bool? GetBool(JsonElement root, string prop)
    {
        return root.TryGetProperty(prop, out var el) ? el.GetBoolean() : null;
    }

    private static int? ParseShadowColor(JsonElement root)
    {
        if (!root.TryGetProperty("shadow_color", out var sc)) return null;
        if (sc.ValueKind == JsonValueKind.Number) return sc.GetInt32();
        if (sc.ValueKind != JsonValueKind.Array) throw new JsonException("Invalid shadow_color format.");
        if (sc.GetArrayLength() != 4) throw new JsonException("shadow_color array must have exactly 4 elements.");

        var elements = sc.EnumerateArray();
        elements.MoveNext(); byte r = Convert.ToByte(elements.Current.GetSingle() * 255);
        elements.MoveNext(); byte g = Convert.ToByte(elements.Current.GetSingle() * 255);
        elements.MoveNext(); byte b = Convert.ToByte(elements.Current.GetSingle() * 255);
        elements.MoveNext(); byte a = Convert.ToByte(elements.Current.GetSingle() * 255);
        return (a << 24) | (r << 16) | (g << 8) | b;
    }

}
