using Sharpmine.Server.Protocol.DataTypes;

namespace Sharpmine.Server.Protocol.Packets.Abstract.Clientbound;

public abstract partial record DisconnectPacket
{

    public TextComponent Reason { get; init; } = null!;

    public void SerializeContent(Stream stream, BinaryWriter writer)
    {
        throw new NotImplementedException("NBT serialization");
    }

}
