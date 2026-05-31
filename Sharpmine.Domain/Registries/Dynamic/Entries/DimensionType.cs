namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public record DimensionType(
    bool Ultrawarm,
    bool Natural,
    double CoordinateScale,
    bool HasSkylight,
    bool HasCeiling,
    float AmbientLight,
    long? FixedTime,
    int MonsterSpawnLightLevel,
    int MonsterSpawnBlockLightLimit,
    Identifier Infiniburn,
    Identifier Effects,
    int MinY,
    int Height,
    int LogicalHeight);
