using Sharpmine.Domain.Registries.Dynamic.SubTypes.Structures;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public sealed record CowVariant(
    Identifier AssetId,
    string Model,
    SpawnCondition[]? SpawnConditions);
