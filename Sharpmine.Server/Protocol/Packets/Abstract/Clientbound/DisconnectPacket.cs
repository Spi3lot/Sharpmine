using System.Buffers;

using Sharpmine.Server.Protocol.DataTypes;

namespace Sharpmine.Server.Protocol.Packets.Abstract.Clientbound;

public abstract partial record DisconnectPacket
{

    public TextComponent Reason { get; init; } = null!;

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        throw new NotImplementedException("NBT serialization");
    }

}
