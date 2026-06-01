namespace Sharpmine.Domain.Registries.Dynamic.SubTypes.Structures;

public readonly record struct FloatRange(
    float? Min,
    float? Max);
