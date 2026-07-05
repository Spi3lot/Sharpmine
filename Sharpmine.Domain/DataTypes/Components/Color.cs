using System.Runtime.CompilerServices;

namespace Sharpmine.Domain.DataTypes.Components;

// https://minecraft.wiki/w/Formatting_codes
public readonly record struct Color(
    string Code,
    string ForegroundHex, // TODO: Maybe store as int, or both string and int
    string BackgroundHex, // TODO: Maybe store as int, or both string and int
    string Ansi, 
    [CallerMemberName] string? Name = null) // TODO: Test what this results in
{

    public static readonly Color Black = new("§0", "#000000", "#000000", "\e[0;30m");

    // TODO: Implement §1 through §f

    // TODO: Static methods for color conversion?

}
