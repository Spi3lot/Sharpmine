using System.Buffers;

using Sharpmine.Domain;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;
using Sharpmine.Server.Infrastructure.Protocol.Packets;

namespace Sharpmine.Server.Infrastructure.Protocol;

public class PacketSerializer
{

    /// <summary>
    /// Serializes a clientbound packet to the given writer
    /// </summary>
    /// <returns>The length of the packet content in bytes, including its ID but excluding the length prefix</returns>
    public int Serialize(IClientboundPacket packet, IBufferWriter<byte> writer)
    {
        using var pooledWriter = new PooledByteBufferWriter();
        pooledWriter.WriteVarInt(packet.Id);
        packet.SerializeContent(pooledWriter);

        int packetLength = pooledWriter.WrittenCount;
        writer.WriteVarInt(packetLength);
        writer.Write(pooledWriter.WrittenSpan);
        return packetLength;
    }

}
