using System.Text.RegularExpressions;

namespace Sharpmine.Domain;

public readonly partial record struct Identifier
{

    public Identifier(string value) : this(
        (value.Contains(':')) ? value.Split(':')[0] : "minecraft",
        (value.Contains(':')) ? value.Split(':')[1] : value)
    {
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

    [GeneratedRegex("^[a-z0-9_.-]+$")]
    private static partial Regex NamespaceRegex { get; }

    [GeneratedRegex("^[a-z0-9/_.-]+$")]
    private static partial Regex PathRegex { get; }

    public static implicit operator Identifier(string value) => new(value);

    public static implicit operator string(Identifier id) => id.ToString();

    public override string ToString() => $"{Namespace}:{Path}";

}
