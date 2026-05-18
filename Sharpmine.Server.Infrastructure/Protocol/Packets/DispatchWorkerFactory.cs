using System.Threading.Channels;

using Microsoft.Extensions.DependencyInjection;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets;

public class DispatchWorkerFactory(IServiceProvider services)
{

    public DispatchWorker Create(
        Channel<IServerboundPacket> channel,
        ClientHandler client)
    {
        return ActivatorUtilities.CreateInstance<DispatchWorker>(services, channel, client);
    }

}
