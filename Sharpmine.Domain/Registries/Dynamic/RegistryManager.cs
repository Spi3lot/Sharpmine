using Sharpmine.Domain.Registries.Dynamic.Entries;

namespace Sharpmine.Domain.Registries.Dynamic;

public class RegistryManager
{

    public DynamicRegistry<BannerPattern> BannerPatterns { get; } = new();

    public DynamicRegistry<ChatType> ChatTypes { get; } = new();

    public DynamicRegistry<DamageType> DamageTypes { get; } = new();

    public DynamicRegistry<Dialog> Dialogs { get; } = new();

    public DynamicRegistry<DimensionType> DimensionTypes { get; } = new();

    public DynamicRegistry<Enchantment> Enchantments { get; } = new();

    public DynamicRegistry<Instrument> Instruments { get; } = new();

    public DynamicRegistry<JukeboxSong> JukeboxSongs { get; } = new();

    public DynamicRegistry<PaintingVariant> PaintingVariants { get; } = new();

    public DynamicRegistry<TrimMaterial> TrimMaterials { get; } = new();

    public DynamicRegistry<TrimPattern> TrimPatterns { get; } = new();

    public DynamicRegistry<Biome> Biomes { get; } = new();

    public DynamicRegistry<CatVariant> CatVariants { get; } = new();

    public DynamicRegistry<ChickenVariant> ChickenVariants { get; } = new();

    public DynamicRegistry<CowVariant> CowVariants { get; } = new();

    public DynamicRegistry<FrogVariant> FrogVariants { get; } = new();

    public DynamicRegistry<PigVariant> PigVariants { get; } = new();

    public DynamicRegistry<WolfVariant> WolfVariants { get; } = new();

    public DynamicRegistry<WolfSoundVariant> WolfSoundVariants { get; } = new();

}
