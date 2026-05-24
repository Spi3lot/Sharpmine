using Sharpmine.Domain.Tags;
using Sharpmine.Server.Infrastructure.Protocol.DataTypes;
using Sharpmine.Server.Infrastructure.Protocol.Packets;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Configuration.Clientbound;
using Sharpmine.Server.Infrastructure.Protocol.Versions;

namespace Sharpmine.Server.Infrastructure.Protocol;

public class NetworkTagCache
{

    public NetworkTagCache(
        TagCache tagCache,
        IProtocol protocol,
        ProtocolRegistryManifest protocolRegistryManifest)
    {
        List<TaggedRegistry> registries = [];

        foreach (var (registryId, registryTags) in tagCache.TagsByRegistry.Values)
        {
            bool isSyncedDynamic = protocol.SynchronizedRegistryIds.Contains(registryId);
            bool isStatic = protocolRegistryManifest.IsStaticRegistry(registryId);

            if (!isSyncedDynamic && !isStatic)
            {
                continue;
            }

            List<RegistryTag> tags = [];

            foreach (var tag in registryTags)
            {
                List<int> protocolIds = [];

                foreach (var entryId in tag.Values)
                {
                    if (protocolRegistryManifest.TryGetId(registryId, entryId, out int protocolId))
                    {
                        protocolIds.Add(protocolId);
                    }
                }

                tags.Add(new RegistryTag(tag.TagName, [.. protocolIds]));
            }

            if (tags.Count > 0)
            {
                registries.Add(new TaggedRegistry(registryId, [.. tags]));
            }
        }

        var packet = new UpdateTagsPacket { TaggedRegistries = [.. registries] };
        Packet = PreSerializedPacket.Generate(packet);
    }

    public PreSerializedPacket<UpdateTagsPacket> Packet { get; }

}
