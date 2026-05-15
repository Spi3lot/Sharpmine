using System.Threading.Channels;

namespace Sharpmine.Server.Core.Protocol.Packets;

public class DispatchWorker(
    Channel<IServerboundPacket> channel,
    ClientHandler client,
    PacketDispatcher packetDispatcher) : ChannelWorker<IServerboundPacket>(channel)
{

    protected override ValueTask ProcessAsync(IServerboundPacket currentItem, CancellationToken cancellationToken)
    {
        if (packetDispatcher.DispatchAsync(currentItem, client, cancellationToken) is not { } handleTask)
        {
            client.LogNoPacketHandler(currentItem);
            return ValueTask.CompletedTask;
        }

        return handleTask;
    }

    protected override Task OnErrorAsync(Exception ex, IServerboundPacket? currentItem)
    {
        client.LogErrorWhileHandlingPacket(ex, currentItem);
        return client.DisconnectAsync("Internal server error during packet handling");
    }

}
