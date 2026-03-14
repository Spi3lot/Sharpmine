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

    public Score? Score { get; init; }

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

    public bool IsLiteral() => this is
    {
        Type: null or ContentType.Text,
        Text: not null,
        Translate: null,
        Fallback: null,
        With: null,
        Score: null,
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
        ShadowColor: null,
        ClickEvent: null,
        HoverEvent: null,
        Extra: null
    };

    public bool IsList() => Extra is not null;

    public string AsLiteral() => Text!;

    public List<TextComponent> AsList() => [this with { Extra = null }, .. Extra ?? []];

    public static TextComponent Literal(string text) => new() { Text = text };

    public static TextComponent List(List<TextComponent>? list) => (list?.Count > 0)
        ? list[0] with { Extra = [.. list[0].Extra ?? [], .. list[1..]] }
        : new TextComponent();

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