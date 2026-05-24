using System.Text.RegularExpressions;

namespace Sharpmine.Domain;

public readonly partial record struct Identifier
{

    public Identifier(string value)
    {
        string[] parts = value.Split(':');

        switch (parts.Length)
        {
            case 1:
                Namespace = "minecraft";
                Path = parts[0];
                break;
            case 2:
                Namespace = parts[0];
                Path = parts[1];
                break;
            default:
                throw new ArgumentException($"Invalid identifier format: {value}");
        }

        if (!NamespaceRegex.IsMatch(Namespace) || !PathRegex.IsMatch(Path))
        {
            throw new ArgumentException($"Invalid identifier format: {value}");
        }
    }

    public Identifier(string @namespace, string path)
    {
        if (!NamespaceRegex.IsMatch(@namespace) || !PathRegex.IsMatch(path))
        {
            throw new ArgumentException($"Invalid identifier format: {@namespace}:{path}");
        }

        Namespace = @namespace;
        Path = path;
    }

    public string Namespace { get; }

    public string Path { get; }

    [GeneratedRegex("[a-z0-9.-_]")]
    private static partial Regex NamespaceRegex { get; }

    [GeneratedRegex("[a-z0-9.-_/]")]
    private static partial Regex PathRegex { get; }

    public static implicit operator Identifier(string value) => new(value);

    public static implicit operator string(Identifier id) => id.ToString();

    public override string ToString() => $"{Namespace}:{Path}";

}
