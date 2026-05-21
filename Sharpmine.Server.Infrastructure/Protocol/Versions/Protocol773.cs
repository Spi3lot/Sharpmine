using System.Collections.Immutable;

namespace Sharpmine.Server.Infrastructure.Protocol.Versions;

public class Protocol773 : IProtocol
{

    public int VersionNumber => 773;

    public string ReleaseName => "1.21.10";

    public ImmutableArray<string> SynchronizedRegistryIds { get; } =
    [
        "minecraft:banner_pattern",
        "minecraft:chat_type",
        "minecraft:damage_type",
        "minecraft:dialog",
        "minecraft:dimension_type",
        "minecraft:enchantment",
        "minecraft:instrument",
        "minecraft:jukebox_song",
        "minecraft:painting_variant",
        "minecraft:trim_material",
        "minecraft:trim_pattern",
        "minecraft:worldgen/biome",
        "minecraft:cat_variant",
        "minecraft:chicken_variant",
        "minecraft:cow_variant",
        "minecraft:frog_variant",
        "minecraft:pig_variant",
        "minecraft:wolf_variant",
        "minecraft:wolf_sound_variant"
    ];

}
