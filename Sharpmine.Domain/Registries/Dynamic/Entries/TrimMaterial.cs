using Sharpmine.Domain.DataTypes;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public record TrimMaterial(
    string AssetName,
    EquipmentAssetId OverrideArmorAssets,
    TextComponent Description);
