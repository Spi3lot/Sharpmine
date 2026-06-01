using Sharpmine.Domain.Registries.Dynamic.SubTypes.Structures;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public record ChickenVariant(
    Identifier AssetId,
    string Model,
    SpawnCondition[]? SpawnConditions);
