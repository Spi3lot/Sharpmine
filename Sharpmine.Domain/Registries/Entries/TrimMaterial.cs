using Sharpmine.Domain.Registries.SubTypes.Structures;

namespace Sharpmine.Domain.Registries.Entries;

public record TrimMaterial(
    string AssetName,
    Identifier Ingredient,
    float ItemModelIndex,
    TrimDescription Description);
