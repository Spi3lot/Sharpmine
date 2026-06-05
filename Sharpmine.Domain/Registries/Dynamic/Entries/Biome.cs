using Sharpmine.Domain.Registries.Dynamic.SubTypes.Structures;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public sealed record Biome(
    float Temperature,
    float Downfall,
    bool HasPrecipitation,
    BiomeEffects Effects);
