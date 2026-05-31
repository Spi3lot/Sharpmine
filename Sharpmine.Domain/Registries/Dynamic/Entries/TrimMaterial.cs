using Sharpmine.Domain.Registries.Dynamic.SubTypes.Structures;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public record TrimMaterial(
    string AssetName,
    Identifier Ingredient,
    float ItemModelIndex,
    TrimDescription Description);
