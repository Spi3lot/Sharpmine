using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;
using Sharpmine.Server.Infrastructure.Protocol.Packets;

namespace Sharpmine.Server.Infrastructure.Protocol;

public class PacketSerializer
{

    private readonly ArrayBufferWriter<byte> _arrayBufferWriter = new();

    /// <summary>
    /// Serializes a clientbound packet to the given writer
    /// </summary>
    /// <returns>The length of the packet content in bytes, including its ID but excluding the length prefix</returns>
    public int Serialize(IClientboundPacket packet, IBufferWriter<byte> writer)
    {
        _arrayBufferWriter.Clear();
        _arrayBufferWriter.WriteVarInt(packet.Id);
        packet.SerializeContent(_arrayBufferWriter);

        int packetLength = _arrayBufferWriter.WrittenCount;
        writer.WriteVarInt(packetLength);
        writer.Write(_arrayBufferWriter.WrittenSpan);
        return packetLength;
    }

}
