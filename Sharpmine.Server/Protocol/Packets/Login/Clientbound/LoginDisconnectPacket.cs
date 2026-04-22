using Sharpmine.Server.Protocol.DataTypes;
using Sharpmine.Server.Protocol.Extensions;

namespace Sharpmine.Server.Protocol.Packets.Login.Clientbound;

public partial record LoginDisconnectPacket(TextComponent Reason)
{

    public void SerializeContent(Stream stream, BinaryWriter writer)
    {
        stream.WriteJson(Reason);
    }

}
