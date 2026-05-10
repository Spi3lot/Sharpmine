using Microsoft.Extensions.Logging;

using Sharpmine.Server.Security;

namespace Sharpmine.Server.Protocol;

public class ClientHandlerFactory(
    PacketDispatcher packetDispatcher,
    ServerCapacityManager serverCapacityManager,
    ILoggerFactory loggerFactory
)
{

    public ClientHandler Create(string ip, TcpClient client)
    {
        var clientHandlerLogger = loggerFactory.CreateLogger<ClientHandler>();
        var packetLogger = loggerFactory.CreateLogger<PacketTransceiver>();
        var packetTransceiver = new PacketTransceiver(packetLogger);

        return new ClientHandler(
            ip,
            client,
            packetTransceiver,
            packetDispatcher,
            serverCapacityManager,
            clientHandlerLogger);
    }

}
