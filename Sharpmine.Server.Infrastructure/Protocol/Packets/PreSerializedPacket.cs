using System.Buffers;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets;

public class PreSerializedPacket : IClientboundPacket
{

    private PreSerializedPacket(Type underlyingPacketType, ProtocolState state, int id, byte[] content)
    {
        UnderlyingPacketType = underlyingPacketType;
        State = state;
        Id = id;
        Content = content;
    }

    public Type UnderlyingPacketType { get; }

    public ProtocolState State { get; }

    public int Id { get; }

    public byte[] Content { get; }

    public static PreSerializedPacket Generate<TPacket>(TPacket packet) where TPacket : IClientboundPacket
    {
        var writer = new ArrayBufferWriter<byte>();
        packet.SerializeContent(writer);
        return new PreSerializedPacket(typeof(TPacket), packet.State, packet.Id, writer.WrittenSpan.ToArray());
    }

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.Write(Content);
    }

    public override string ToString()
    {
        return $"Pre-Serialized {UnderlyingPacketType.Name} {{ {nameof(State)} = {State}, {nameof(Id)} = {Id} }}";
    }

}
