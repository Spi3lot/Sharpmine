namespace Sharpmine.Domain.Registries.Dynamic.SubTypes.Structures;

public readonly record struct SpawnCondition(
    Condition Condition,
    int Priority);
