using System.Runtime.CompilerServices;

namespace Sharpmine.Domain.DataTypes.Components;

// https://minecraft.wiki/w/Formatting_codes
public readonly struct Color
{

    private static uint _code;

    private Color(
        int foreground,
        int background,
        string ansi,
        [CallerMemberName] string? name = null)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(_code, 36u);
        Foreground = foreground;
        Background = background;
        ForegroundHex = foreground.ToString("X6");
        BackgroundHex = background.ToString("X6");
        Ansi = ansi;
        Name = PascalCaseToSnakeCase(name!);
        Code = "§" + Convert.ToChar((_code < 10) ? _code : 'a' + _code - 10);
        _code++;
    }

    public int Foreground { get; }

    public int Background { get; }

    public string ForegroundHex { get; }

    public string BackgroundHex { get; }

    public string Ansi { get; }

    public string Name { get; }

    public string Code { get; }

    public static readonly Color Black = new(0x000000, 0x000000, "\e[0;30m");

    public static readonly Color DarkBlue = new(0x0000AA, 0x00002A, "\e[0;34m");

    public static readonly Color DarkGreen = new(0x00AA00, 0x002A00, "\e[0;32m");

    public static readonly Color DarkAqua = new(0x00AAAA, 0x002A2A, "\e[0;36m");

    public static readonly Color DarkRed = new(0xAA0000, 0x2A0000, "\e[0;31m");

    public static readonly Color DarkPurple = new(0xAA00AA, 0x2A002A, "\e[0;35m");

    public static readonly Color Gold = new(0xFFAA00, 0x3E2A00, "\e[0;33m");

    public static readonly Color Gray = new(0xAAAAAA, 0x2A2A2A, "\e[0;37m");

    public static readonly Color DarkGray = new(0x555555, 0x151515, "\e[0;90m");

    public static readonly Color Blue = new(0x5555FF, 0x15153F, "\e[0;94m");

    public static readonly Color Green = new(0x55FF55, 0x153F15, "\e[0;92m");

    public static readonly Color Aqua = new(0x55FFFF, 0x153F3F, "\e[0;96m");

    public static readonly Color Red = new(0xFF5555, 0x3F1515, "\e[0;91m");

    public static readonly Color LightPurple = new(0xFF55FF, 0x3F153F, "\e[0;95m");

    public static readonly Color Yellow = new(0xFFFF55, 0x3F3F15, "\e[0;93m");

    public static readonly Color White = new(0xFFFFFF, 0x3F3F3F, "\e[0;97m");

    private static string PascalCaseToSnakeCase(string input)
    {
        return string.Concat(input.Select((c, i) => (i > 0) && char.IsUpper(c) ? $"_{c}" : $"{c}")).ToLowerInvariant();
    }

}
