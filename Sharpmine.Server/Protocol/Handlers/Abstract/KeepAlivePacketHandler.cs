using Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

namespace Sharpmine.Server.Protocol.Handlers.Abstract;

public class KeepAlivePacketHandler : IPacketHandler<KeepAlivePacket>
{

    public ValueTask HandleAsync(
        KeepAlivePacket packet,
        ClientHandler client,
        CancellationToken cancellationToken)
    {
        // TODO: Check whether the client responded with the same packet
        return ValueTask.CompletedTask;
    }

}
