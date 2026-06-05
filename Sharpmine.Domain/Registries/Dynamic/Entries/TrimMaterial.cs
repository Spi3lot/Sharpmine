using Sharpmine.Domain.DataTypes;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public sealed record TrimMaterial(
    string AssetName,
    TextComponent Description,
    IReadOnlyDictionary<string, string>? OverrideArmorAssets);
