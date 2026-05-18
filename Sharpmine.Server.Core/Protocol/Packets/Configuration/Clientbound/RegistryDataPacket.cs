using System.Buffers;

using Sharpmine.Server.Core.Protocol.DataTypes;
using Sharpmine.Server.Core.Protocol.Extensions;

namespace Sharpmine.Server.Core.Protocol.Packets.Configuration.Clientbound;

public partial record RegistryDataPacket(string RegistryId, RegistryEntry[] Entries)
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteString(RegistryId);
        writer.WritePrefixedArray(Entries);
    }

}
