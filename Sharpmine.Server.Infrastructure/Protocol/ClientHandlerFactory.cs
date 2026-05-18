using Microsoft.Extensions.DependencyInjection;

namespace Sharpmine.Server.Infrastructure.Protocol;

public class ClientHandlerFactory(IServiceProvider services)
{

    public ClientHandler Create(string ip, TcpClient client)
    {
        return ActivatorUtilities.CreateInstance<ClientHandler>(services, ip, client);
    }

}
