using System.Buffers;

using Sharpmine.Server.Core.Protocol.DataTypes;

namespace Sharpmine.Server.Core.Protocol.Packets.Abstract.Clientbound;

public abstract partial record DisconnectPacket
{

    public TextComponent Reason { get; init; } = null!;

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        throw new NotImplementedException("NBT serialization");
    }

}
