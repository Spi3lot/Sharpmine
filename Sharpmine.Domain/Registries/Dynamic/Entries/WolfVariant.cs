using Sharpmine.Domain.Registries.Dynamic.SubTypes.Structures;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public record WolfVariant(
    WolfAssets Assets,
    SpawnCondition[]? SpawnConditions);
