using Microsoft.Extensions.DependencyInjection;

namespace Sharpmine.Server.Core.Protocol;

public class ClientHandlerFactory(IServiceProvider services)
{

    public ClientHandler Create(string ip, TcpClient client)
    {
        return ActivatorUtilities.CreateInstance<ClientHandler>(services, ip, client);
    }

}
