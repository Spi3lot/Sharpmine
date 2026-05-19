using System.Buffers;

using Optional;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets;

public static class PreSerializedPacket
{

    public static PreSerializedPacket<TPacket> Generate<TPacket>(TPacket packet, bool retainUnderlyingPacket = false)
        where TPacket : IClientboundPacket
    {
        var writer = new ArrayBufferWriter<byte>();
        packet.SerializeContent(writer);

        return new PreSerializedPacket<TPacket>(
            packet.SomeWhen(_ => retainUnderlyingPacket),
            packet.State,
            packet.Id,
            writer.WrittenSpan.ToArray());
    }

}
