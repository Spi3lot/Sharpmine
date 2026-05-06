using Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

namespace Sharpmine.Server.Protocol.Handlers.Abstract;

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
