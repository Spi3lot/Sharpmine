using Sharpmine.Domain.Registries.SubTypes.Structures;

namespace Sharpmine.Domain.Registries.Entries;

public record Biome(
    float Temperature,
    float Downfall,
    bool HasPrecipitation,
    BiomeEffects Effects);
