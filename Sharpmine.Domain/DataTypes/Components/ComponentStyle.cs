namespace Sharpmine.Domain.DataTypes.Components;

public readonly record struct ComponentStyle(
    string? Color,
    string? Font,
    bool? Bold,
    bool? Italic,
    bool? Underlined,
    bool? Strikethrough,
    bool? Obfuscated,
    int? ShadowColor,
    string? Insertion)
{

    public bool IsEmpty => this == default;

}
