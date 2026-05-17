using System.IO.Pipelines;

using Microsoft.Extensions.Logging;

using Sharpmine.Server.Core.Protocol.Packets;

namespace Sharpmine.Server.Core.Protocol;

public partial class PacketTransmitter(
    PacketSerializer packetSerializer,
    ILogger<PacketTransmitter> logger)
{

    public async ValueTask<bool> TransmitAsync(
        IClientboundPacket packet,
        PipeWriter pipeWriter,
        CancellationToken cancellationToken)
    {
        try
        {
            int length = packetSerializer.Serialize(packet, pipeWriter);
            LogTransmittingPacket(packet, length);
        }
        catch (NotImplementedException)
        {
            LogSerializeNotImplemented(packet);
            return true;
        }

        var result = await pipeWriter.FlushAsync(cancellationToken);
        return !(result.IsCompleted || result.IsCanceled);
    }

}
