using System.Buffers;

using Sharpmine.Server.Protocol.DataTypes;
using Sharpmine.Server.Protocol.Extensions;

namespace Sharpmine.Server.Protocol.Packets.Login.Clientbound;

public partial record LoginFinishedPacket(GameProfile Profile)
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.Write(Profile);
    }

}
