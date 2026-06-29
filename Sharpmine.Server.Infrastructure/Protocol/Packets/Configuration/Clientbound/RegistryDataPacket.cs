using System.Buffers;

using Sharpmine.Domain;
using Sharpmine.Domain.DataTypes;
using Sharpmine.Domain.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Configuration.Clientbound;

public partial record RegistryDataPacket(Identifier RegistryId, RegistryEntry[] Entries)
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteIdentifier(RegistryId);
        writer.WritePrefixedArray(Entries);
    }

}
