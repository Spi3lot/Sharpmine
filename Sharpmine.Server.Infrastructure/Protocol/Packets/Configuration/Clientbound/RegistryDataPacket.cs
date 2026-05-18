using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;
using Sharpmine.Server.Infrastructure.Protocol.DataTypes;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Configuration.Clientbound;

public partial record RegistryDataPacket(string RegistryId, RegistryEntry[] Entries)
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteString(RegistryId);
        writer.WritePrefixedArray(Entries);
    }

}
