using Sharpmine.Domain;
using Sharpmine.Domain.Tags;
using Sharpmine.Server.Infrastructure.Protocol.DataTypes;
using Sharpmine.Server.Infrastructure.Protocol.Packets;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Configuration.Clientbound;

namespace Sharpmine.Server.Infrastructure.Protocol;

public class NetworkTagCache
{

    public NetworkTagCache(TagCache cache, ProtocolIdMap idMap)
    {
        List<TaggedRegistry> protocolRegistries = [];

        foreach (var registry in cache.Registries.Values)
        {
            List<RegistryTag> protocolTags = [];

            foreach (var tag in registry.Tags)
            {
                List<int> protocolIds = [];

                foreach (string stringValue in tag.Values)
                {
                    if (idMap.TryGetId(registry.RegistryId, stringValue, out int protocolId))
                    {
                        protocolIds.Add(protocolId);
                    }
                }

                protocolTags.Add(new RegistryTag(tag.TagName, [.. protocolIds]));
            }

            if (protocolTags.Count > 0)
            {
                protocolRegistries.Add(new TaggedRegistry(registry.RegistryId, [.. protocolTags]));
            }
        }

        var packet = new UpdateTagsPacket { TaggedRegistries = [.. protocolRegistries] };
        Packet = PreSerializedPacket.Generate(packet);
    }

    public PreSerializedPacket<UpdateTagsPacket> Packet { get; }

}
