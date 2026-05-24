using System.Collections.Immutable;

using Sharpmine.Domain;

namespace Sharpmine.Server.Infrastructure.Protocol.Versions;

public class Protocol773 : IProtocol
{

    public int VersionNumber => 773;

    public string ReleaseName => "1.21.10";

    public ImmutableArray<Identifier> SynchronizedRegistryIds { get; } =
    [
        "banner_pattern",
        "chat_type",
        "damage_type",
        "dialog",
        "dimension_type",
        "enchantment",
        "instrument",
        "jukebox_song",
        "painting_variant",
        "trim_material",
        "trim_pattern",
        "worldgen/biome",
        "cat_variant",
        "chicken_variant",
        "cow_variant",
        "frog_variant",
        "pig_variant",
        "wolf_variant",
        "wolf_sound_variant"
    ];

}
