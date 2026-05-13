using System.IO.Pipelines;
using System.Threading.Channels;

namespace Sharpmine.Server.Protocol.Packets;

public class TransmissionWorker(
    Channel<IClientboundPacket> channel,
    ClientHandler client,
    PipeWriter pipeWriter,
    PacketTransmitter packetTransmitter) : ChannelWorker<IClientboundPacket>(channel)
{

    protected override Task ProcessAsync(IClientboundPacket currentItem, CancellationToken cancellationToken)
    {
        return packetTransmitter.TransmitAsync(currentItem, pipeWriter, cancellationToken);
    }

    protected override Task OnErrorAsync(Exception ex, IClientboundPacket? currentItem)
    {
        client.LogErrorWhileTransmittingPacket(ex, currentItem);
        return client.AbortForcefullyAsync();
    }

}
