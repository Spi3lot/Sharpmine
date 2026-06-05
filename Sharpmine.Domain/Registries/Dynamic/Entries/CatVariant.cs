using Sharpmine.Domain.Registries.Dynamic.SubTypes.Structures;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public sealed record CatVariant(
    Identifier AssetId,
    SpawnCondition[]? SpawnConditions);
