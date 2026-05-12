using System.Buffers;

using Sharpmine.Server.Protocol.DataTypes;
using Sharpmine.Server.Protocol.Extensions;

namespace Sharpmine.Server.Protocol.Packets.Status.Clientbound;

public partial record StatusResponsePacket(ServerStatus Status)
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteJsonString(Status);
    }

}
