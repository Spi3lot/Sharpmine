using System.Buffers;

using Sharpmine.Domain.DataTypes;
using Sharpmine.Domain.Extensions;


namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Login.Clientbound;

public partial record LoginFinishedPacket(in GameProfile Profile)
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.Write(Profile);
    }

}
