using System.Collections.Immutable;

using Sharpmine.Domain;
using Sharpmine.Server.Infrastructure.Protocol.Packets;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Configuration.Clientbound;

namespace Sharpmine.Server.Infrastructure.Protocol;

public class NetworkRegistryCache
{

    public NetworkRegistryCache(RegistryCache cache)
    {
        List<PreSerializedPacket> packets = [];

        foreach (var folder in cache.Registries)
        {
            var entries =
                from entry in folder.Entries
                select new DataTypes.RegistryEntry(entry.EntryId, entry.Data);

            var packet = new RegistryDataPacket(folder.RegistryId, entries.ToArray());
            packets.Add(PreSerializedPacket.Generate(packet));
        }

        Packets = [.. packets];
    }

    public ImmutableArray<PreSerializedPacket> Packets { get; }

}
