using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;
using Sharpmine.Server.Infrastructure.Protocol.DataTypes;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Status.Clientbound;

public partial record StatusResponsePacket(ServerStatus Status)
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteJsonString(Status);
    }

}
