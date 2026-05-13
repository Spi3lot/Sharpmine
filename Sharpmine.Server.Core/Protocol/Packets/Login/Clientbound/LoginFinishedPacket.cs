using System.Buffers;

using Sharpmine.Server.Core.Protocol.DataTypes;
using Sharpmine.Server.Core.Protocol.Extensions;


namespace Sharpmine.Server.Core.Protocol.Packets.Login.Clientbound;

public partial record LoginFinishedPacket(GameProfile Profile)
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.Write(Profile);
    }

}
