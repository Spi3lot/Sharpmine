namespace Sharpmine.Domain.Registries.Dynamic.SubTypes.Structures;

public readonly record struct Condition(
    Identifier Type,
    string? Biomes,
    string? Structures,
    FloatRange? Range);
