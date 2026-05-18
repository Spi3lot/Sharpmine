using System.IO.Pipelines;
using System.Threading.Channels;

using Microsoft.Extensions.DependencyInjection;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets;

public class TransmissionWorkerFactory(IServiceProvider services)
{

    public TransmissionWorker Create(
        Channel<IClientboundPacket> channel,
        ClientHandler client,
        PipeWriter pipeWriter)
    {
        return ActivatorUtilities.CreateInstance<TransmissionWorker>(services, channel, client, pipeWriter);
    }

}
