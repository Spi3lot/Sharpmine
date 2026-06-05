using Sharpmine.Domain.Providers.Int;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public sealed record DimensionType(
    float AmbientLight,
    bool BedWorks,
    int CloudHeight,
    double CoordinateScale,
    Identifier Effects,
    long? FixedTime,
    bool HasCeiling,
    bool HasRaids,
    bool HasSkylight,
    int Height,
    Identifier Infiniburn,
    int LogicalHeight,
    int MinY,
    int MonsterSpawnBlockLightLimit,
    IIntProvider MonsterSpawnLightLevel,
    bool Natural,
    bool PiglinSafe,
    bool RespawnAnchorWorks,
    bool Ultrawarm);
