using System.IO.Pipelines;
using System.Threading.Channels;

namespace Sharpmine.Server.Core.Protocol.Packets;

public class TransmissionWorker(
    Channel<IClientboundPacket> channel,
    ClientHandler client,
    PipeWriter pipeWriter,
    PacketTransmitter packetTransmitter) : ChannelWorker<IClientboundPacket>(channel)
{

    protected override async ValueTask ProcessAsync(IClientboundPacket currentItem, CancellationToken cancellationToken)
    {
        if (!await packetTransmitter.TransmitAsync(currentItem, pipeWriter, cancellationToken))
        {
            await client.AbortForcefullyAsync();
        }
    }

    protected override Task OnErrorAsync(Exception ex, IClientboundPacket? currentItem)
    {
        client.LogErrorWhileTransmittingPacket(ex, currentItem);
        return client.AbortForcefullyAsync();
    }

}
