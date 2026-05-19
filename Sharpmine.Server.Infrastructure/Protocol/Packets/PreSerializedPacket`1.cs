using System.Buffers;

using Optional;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets;

public class PreSerializedPacket<TPacket> : IClientboundPacket where TPacket : IClientboundPacket
{

    internal PreSerializedPacket(Option<TPacket> underlyingPacket, ProtocolState state, int id, byte[] content)
    {
        UnderlyingPacket = underlyingPacket;
        State = state;
        Id = id;
        Content = content;
    }

    public Option<TPacket> UnderlyingPacket { get; }

    public ProtocolState State { get; }

    public int Id { get; }

    public byte[] Content { get; }

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.Write(Content);
    }

    public override string ToString()
    {
        return UnderlyingPacket.Match(
            packet => $"Pre-Serialized {packet}",
            () => $"Pre-Serialized {typeof(TPacket).Name} {{ {nameof(State)} = {State}, {nameof(Id)} = {Id} }}");
    }

}
