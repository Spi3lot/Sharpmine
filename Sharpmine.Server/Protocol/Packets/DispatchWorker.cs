using System.Threading.Channels;

namespace Sharpmine.Server.Protocol.Packets;

public class DispatchWorker(
    Channel<IServerboundPacket> channel,
    ClientHandler client,
    PacketDispatcher packetDispatcher) : ChannelWorker<IServerboundPacket>(channel)
{

    protected override async Task ProcessAsync(IServerboundPacket currentItem, CancellationToken cancellationToken)
    {
        if (packetDispatcher.DispatchAsync(currentItem, client, cancellationToken) is not { } handleTask)
        {
            client.LogNoPacketHandler(currentItem);
            return;
        }

        await handleTask;
    }

    protected override Task OnErrorAsync(Exception ex, IServerboundPacket? currentItem)
    {
        client.LogErrorWhileHandlingPacket(ex, currentItem);
        return client.DisconnectAsync("Internal server error during packet handling");
    }

}
