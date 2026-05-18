using Sharpmine.Server.Infrastructure.Protocol.Packets.Abstract.Serverbound;

namespace Sharpmine.Server.Infrastructure.Protocol.Handlers.Abstract;

public class CustomPayloadPacketHandler : IPacketHandler<CustomPayloadPacket>
{

    public ValueTask HandleAsync(
        CustomPayloadPacket packet,
        ClientHandler client,
        CancellationToken cancellationToken)
    {
        // TODO: Implement
        return ValueTask.CompletedTask;
    }

}
