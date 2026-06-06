using Sharpmine.Domain.DataTypes.Components;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public sealed record TrimMaterial(
    string AssetName,
    Component Description,
    IReadOnlyDictionary<string, string>? OverrideArmorAssets);
