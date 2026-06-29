using System.Buffers;

using Sharpmine.Domain.DataTypes;
using Sharpmine.Domain.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Status.Clientbound;

public partial record StatusResponsePacket(ServerStatus Status)
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteJsonString(Status);
    }

}
