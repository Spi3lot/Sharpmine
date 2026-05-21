using System.Buffers;

using Optional;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets;

public static class PreSerializedPacket
{

    [ThreadStatic]
    private static ArrayBufferWriter<byte>? _arrayBufferWriter;

    public static PreSerializedPacket<TPacket> Generate<TPacket>(TPacket packet, bool retainUnderlyingPacket = false)
        where TPacket : IClientboundPacket
    {
        _arrayBufferWriter ??= new ArrayBufferWriter<byte>(1024);
        _arrayBufferWriter.Clear();
        packet.SerializeContent(_arrayBufferWriter);

        return new PreSerializedPacket<TPacket>(
            packet.SomeWhen(_ => retainUnderlyingPacket),
            packet.State,
            packet.Id,
            _arrayBufferWriter.WrittenSpan.ToArray());
    }

}
