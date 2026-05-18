using System.Collections.Immutable;

using Sharpmine.Domain;
using Sharpmine.Server.Infrastructure.Protocol.Packets;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Configuration.Clientbound;

namespace Sharpmine.Server.Infrastructure.Protocol;

public class NetworkRegistryCache
{

    private static readonly string[] SynchronizedRegistryIds =
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

    public NetworkRegistryCache(RegistryCache cache)
    {
        List<PreSerializedPacket> packets = [];

        foreach (string registryId in SynchronizedRegistryIds)
        {
            if (!cache.Registries.TryGetValue(registryId, out var registry))
            {
                throw new InvalidOperationException(
                    $"Protocol Synchronization Error: Required registry '{registryId}' has not been loaded. " +
                    "Verify your asset files exist on disk.");
            }

            var entries =
                from entry in registry.Entries
                select new DataTypes.RegistryEntry(entry.EntryId, entry.Data);

            var packet = new RegistryDataPacket(registryId, entries.ToArray());
            packets.Add(PreSerializedPacket.Generate(packet));
        }

        Packets = [.. packets];
    }

    public ImmutableArray<PreSerializedPacket> Packets { get; }

}
