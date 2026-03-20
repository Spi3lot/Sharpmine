using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Sharpmine.Server.Protocol;

namespace Sharpmine.Server;

public class ServerFactory(IServiceProvider serviceProvider)
{

    public ServerService Create(int port)
    {
        var clientHandlerFactory = serviceProvider.GetRequiredService<ClientHandlerFactory>();
        var logger = serviceProvider.GetRequiredService<ILogger<ServerService>>();
        return new ServerService(port, clientHandlerFactory, logger);
    }

}
