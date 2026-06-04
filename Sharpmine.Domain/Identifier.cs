using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Sharpmine.Domain;

[JsonConverter(typeof(IdentifierConverter))]
public readonly partial record struct Identifier
{

    public Identifier(string value)
    {
        IsTag = value.StartsWith('#');
        int startIndex = (IsTag) ? 1 : 0;
        int colonIndex = value.IndexOf(':', startIndex);

        if (colonIndex == -1)
        {
            Namespace = "minecraft";
            Path = value[startIndex..];
        }
        else
        {
            Namespace = value[startIndex..colonIndex];
            Path = value[(colonIndex + 1)..];
        }

        ThrowIfInvalidFormat();
    }

    public Identifier(bool isTag, string @namespace, string path)
    {
        IsTag = isTag;
        Namespace = @namespace;
        Path = path;
        ThrowIfInvalidFormat();
    }

    public static Identifier Minecraft(string path) => new(false, "minecraft", path);

    public static Identifier MinecraftTag(string path) => new(true, "minecraft", path);

    public bool IsTag { get; }

    public string Namespace { get; }

    public string Path { get; }

    private void ThrowIfInvalidFormat()
    {
        if (!NamespaceRegex.IsMatch(Namespace) || !PathRegex.IsMatch(Path))
        {
            throw new ArgumentException("Invalid identifier format: " + ToString());
        }
    }

    [GeneratedRegex("^[a-z0-9_.-]+$")]
    private static partial Regex NamespaceRegex { get; }

    [GeneratedRegex("^[a-z0-9/_.-]+$")]
    private static partial Regex PathRegex { get; }

    public static implicit operator Identifier(string value) => new(value);

    public static implicit operator string(Identifier id) => id.ToString();

    public override string ToString() => $"{(IsTag ? "#" : "")}{Namespace}:{Path}";

}
