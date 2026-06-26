using Sharpmine.Domain.DataTypes.Components;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Abstract.Serverbound;

namespace Sharpmine.Server.Infrastructure.Protocol.Handlers.Abstract;

public class KeepAlivePacketHandler : IPacketHandler<KeepAlivePacket>
{

    public ValueTask HandleAsync(
        KeepAlivePacket packet,
        ClientHandler client,
        CancellationToken cancellationToken)
    {
        if (client.CurrentKeepAliveId == packet.KeepAliveId)
        {
            client.WaitingForKeepAlive = false;
        }
        else
        {
            client.DisconnectAsync(new TextComponent("Invalid KeepAliveId")
            {
                Style = new ComponentStyle { Color = "red" }
            });
        }

        return ValueTask.CompletedTask;
    }

}
