using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;
using Sharpmine.Server.Infrastructure.Protocol.DataTypes;


namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Login.Clientbound;

public partial record LoginFinishedPacket(in GameProfile Profile)
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.Write(Profile);
    }

}
