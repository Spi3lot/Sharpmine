using System.Collections.Immutable;

using Sharpmine.Domain.Registries;
using Sharpmine.Server.Infrastructure.Protocol.Packets;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Configuration.Clientbound;
using Sharpmine.Server.Infrastructure.Protocol.Versions;

namespace Sharpmine.Server.Infrastructure.Protocol;

public class NetworkRegistryCache
{

    public NetworkRegistryCache(RegistryCache registryCache, IProtocol protocol)
    {
        List<PreSerializedPacket<RegistryDataPacket>> packets = [];

        foreach (var registryId in protocol.SynchronizedRegistryIds)
        {
            if (!registryCache.Registries.TryGetValue(registryId, out var registry))
            {
                throw new InvalidOperationException($"Protocol Synchronization Error: Required registry '{registryId}' has not been loaded. ");
            }

            var entries =
                from entry in registry.Entries
                select new DataTypes.RegistryEntry(entry.EntryId, entry.Data);

            var packet = new RegistryDataPacket(registryId, entries.ToArray());
            packets.Add(PreSerializedPacket.Generate(packet));
        }

        Packets = [.. packets];
    }

    public ImmutableArray<PreSerializedPacket<RegistryDataPacket>> Packets { get; }

}
