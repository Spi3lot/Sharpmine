using Optional;

using Sharpmine.Domain;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets;

public static class PreSerializedPacket
{

    public static PreSerializedPacket<TPacket> Generate<TPacket>(TPacket packet, bool retainUnderlyingPacket = false)
        where TPacket : IClientboundPacket
    {
        using var pooledWriter = new PooledByteBufferWriter(1024);
        packet.SerializeContent(pooledWriter);

        return new PreSerializedPacket<TPacket>(
            packet.SomeWhen(_ => retainUnderlyingPacket),
            packet.State,
            packet.Id,
            pooledWriter.WrittenSpan.ToArray());
    }

}
