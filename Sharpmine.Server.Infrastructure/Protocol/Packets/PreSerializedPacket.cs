using System.Buffers;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets;

public record PreSerializedPacket(ProtocolState State, int Id, byte[] Content) : IClientboundPacket
{

    public static PreSerializedPacket Generate(IClientboundPacket packet)
    {
        var writer = new ArrayBufferWriter<byte>();
        packet.SerializeContent(writer);
        return new PreSerializedPacket(packet.State, packet.Id, writer.WrittenSpan.ToArray());
    }

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.Write(Content);
    }

}
