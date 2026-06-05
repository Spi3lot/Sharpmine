using Sharpmine.Domain.Registries.Dynamic.SubTypes.Structures;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public sealed record WolfVariant(
    WolfAssets Assets,
    SpawnCondition[]? SpawnConditions);
