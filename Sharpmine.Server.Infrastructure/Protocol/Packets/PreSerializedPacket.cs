using System.Buffers;

using Optional;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets;

public static class PreSerializedPacket
{

    [ThreadStatic]
    private static readonly ArrayBufferWriter<byte> ArrayBufferWriter;

    static PreSerializedPacket()
    {
        ArrayBufferWriter = new ArrayBufferWriter<byte>();
    }

    public static PreSerializedPacket<TPacket> Generate<TPacket>(TPacket packet, bool retainUnderlyingPacket = false)
        where TPacket : IClientboundPacket
    {
        ArrayBufferWriter.Clear();
        packet.SerializeContent(ArrayBufferWriter);

        return new PreSerializedPacket<TPacket>(
            packet.SomeWhen(_ => retainUnderlyingPacket),
            packet.State,
            packet.Id,
            ArrayBufferWriter.WrittenSpan.ToArray());
    }

}
