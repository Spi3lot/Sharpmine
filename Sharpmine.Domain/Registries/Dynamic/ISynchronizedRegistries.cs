using Sharpmine.Domain.Registries.Dynamic.Entries;

namespace Sharpmine.Domain.Registries.Dynamic;

public interface ISynchronizedRegistries
{

    DynamicRegistry<BannerPattern> BannerPatterns { get; }

    DynamicRegistry<ChatType> ChatTypes { get; }

    DynamicRegistry<DamageType> DamageTypes { get; }

    DynamicRegistry<Dialog> Dialogs { get; }

    DynamicRegistry<DimensionType> DimensionTypes { get; }

    DynamicRegistry<Enchantment> Enchantments { get; }

    DynamicRegistry<Instrument> Instruments { get; }

    DynamicRegistry<JukeboxSong> JukeboxSongs { get; }

    DynamicRegistry<PaintingVariant> PaintingVariants { get; }

    DynamicRegistry<TrimMaterial> TrimMaterials { get; }

    DynamicRegistry<TrimPattern> TrimPatterns { get; }

    DynamicRegistry<Biome> Biomes { get; }

    DynamicRegistry<CatVariant> CatVariants { get; }

    DynamicRegistry<ChickenVariant> ChickenVariants { get; }

    DynamicRegistry<CowVariant> CowVariants { get; }

    DynamicRegistry<FrogVariant> FrogVariants { get; }

    DynamicRegistry<PigVariant> PigVariants { get; }

    DynamicRegistry<WolfVariant> WolfVariants { get; }

    DynamicRegistry<WolfSoundVariant> WolfSoundVariants { get; }

}
