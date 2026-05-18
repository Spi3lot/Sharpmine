using System.Threading.Channels;

using Microsoft.Extensions.Logging;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets;

public partial class DispatchWorker(
    Channel<IServerboundPacket> channel,
    ClientHandler client,
    PacketDispatcher packetDispatcher,
    ILogger<DispatchWorker> logger) : ChannelWorker<IServerboundPacket>(channel)
{

    protected override ValueTask ProcessAsync(IServerboundPacket currentItem, CancellationToken cancellationToken)
    {
        if (packetDispatcher.DispatchAsync(currentItem, client, cancellationToken) is not { } handleTask)
        {
            LogNoPacketHandler(currentItem);
            return ValueTask.CompletedTask;
        }

        return handleTask;
    }

    protected override Task OnErrorAsync(Exception ex, IServerboundPacket? currentItem)
    {
        LogErrorWhileHandlingPacket(ex, currentItem);
        return client.DisconnectAsync("Internal server error during packet handling");
    }

}
