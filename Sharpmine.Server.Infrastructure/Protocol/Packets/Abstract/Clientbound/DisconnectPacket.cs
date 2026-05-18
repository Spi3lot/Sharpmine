using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.DataTypes;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Abstract.Clientbound;

public abstract partial record DisconnectPacket
{

    public TextComponent Reason { get; init; } = null!;

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        throw new NotImplementedException("NBT serialization");
    }

}
