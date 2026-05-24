using System.Buffers;

using Sharpmine.Domain;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;
using Sharpmine.Server.Infrastructure.Protocol.DataTypes;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Configuration.Clientbound;

public partial record RegistryDataPacket(Identifier RegistryId, RegistryEntry[] Entries)
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteIdentifier(RegistryId);
        writer.WritePrefixedArray(Entries);
    }

}
