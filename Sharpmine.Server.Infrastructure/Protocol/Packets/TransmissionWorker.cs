using System.IO.Pipelines;
using System.Threading.Channels;

using Microsoft.Extensions.Logging;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets;

public partial class TransmissionWorker(
    Channel<IClientboundPacket> channel,
    ClientHandler client,
    PipeWriter pipeWriter,
    PacketSerializer packetSerializer,
    ILogger<TransmissionWorker> logger) : ChannelWorker<IClientboundPacket>(channel)
{

    protected override async ValueTask ProcessAsync(IClientboundPacket currentItem, CancellationToken cancellationToken)
    {
        do
        {
            try
            {
                int packetLength = packetSerializer.Serialize(currentItem, pipeWriter);
                LogTransmittingPacket(currentItem, packetLength);
            }
            catch (NotImplementedException)
            {
                LogSerializeNotImplemented(currentItem);
            }
        } while (Channel.Reader.TryRead(out currentItem!));

        if (await pipeWriter.FlushAsync(cancellationToken) is not { IsCompleted: false, IsCanceled: false })
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
