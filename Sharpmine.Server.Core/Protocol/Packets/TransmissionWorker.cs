using System.IO.Pipelines;
using System.Threading.Channels;

using Microsoft.Extensions.Logging;

namespace Sharpmine.Server.Core.Protocol.Packets;

public partial class TransmissionWorker(
    Channel<IClientboundPacket> channel,
    ClientHandler client,
    PipeWriter pipeWriter,
    PacketSerializer packetSerializer,
    ILogger<TransmissionWorker> logger) : ChannelWorker<IClientboundPacket>(channel)
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
        LogErrorWhileTransmittingPacket(ex, currentItem);
        return client.AbortForcefullyAsync();
    }

}
