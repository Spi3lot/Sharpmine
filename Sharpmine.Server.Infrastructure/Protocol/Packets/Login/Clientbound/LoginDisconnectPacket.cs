using System.Buffers;

using Sharpmine.Domain.DataTypes.Components;
using Sharpmine.Domain.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Login.Clientbound;

public partial record LoginDisconnectPacket(Component Reason)
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteJsonString(Reason);
    }

}
