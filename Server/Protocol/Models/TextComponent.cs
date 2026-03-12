using System.Text.Json.Serialization;
using Sharpmine.Server.Protocol.Converters;

namespace Sharpmine.Server.Protocol.Models;

[JsonConverter(typeof(TextComponentConverter))]
public record TextComponent
{

    #region Content Types
    public ContentType? Type { get; init; }

    public string? Text { get; init; }

    #region Translate
    public string? Translate { get; init; }

    public string? Fallback { get; init; }

    public List<TextComponent>? With { get; init; }
    #endregion

    public Dictionary<string, string>? Score { get; init; }

    public string? Selector { get; init; }

    public string? Keybind { get; init; }

    public string? Nbt { get; init; }
    #endregion

    #region Formatting
    public string? Color { get; init; }

    public string? Font { get; init; }

    public bool? Bold { get; init; }

    public bool? Italic { get; init; }

    public bool? Underlined { get; init; }

    public bool? Strikethrough { get; init; }

    public bool? Obfuscated { get; init; }

    public string? Insertion { get; init; }

    [JsonPropertyName("shadow_color")]
    public int? ShadowColor { get; init; }
    #endregion

    #region Events
    [JsonPropertyName("click_event")]
    public ClickEvent? ClickEvent { get; init; }

    [JsonPropertyName("hover_event")]
    public HoverEvent? HoverEvent { get; init; }
    #endregion

    public List<TextComponent>? Extra { get; init; }

    public bool IsSimple => this is
    {
        // Text is missing on purpose
        Translate: null,
        With: null,
        Selector: null,
        Keybind: null,
        Color: null,
        Font: null,
        Bold: null,
        Italic: null,
        Underlined: null,
        Strikethrough: null,
        Obfuscated: null,
        Insertion: null,
        // Extra is missing on purpose
    };

    public bool IsLiteral => this is { IsSimple: true, Text: not null, };

    public bool IsList => this is { IsLiteral: true, Extra: not null, };

    public string AsLiteral() => Text!;

    public List<TextComponent> AsList() => [this, ..Extra!];

    public static TextComponent Literal(string text) => new() { Text = text };

    public static TextComponent List(List<TextComponent> list) => new() { Text = list[0].Text!, Extra = list[1..] };

    public enum ContentType
    {

        Text,

        Translatable,

        Score,

        Selector,

        Keybind,

        Nbt,

    }

}

public record ClickEvent; // TODO
public record HoverEvent; // TODO