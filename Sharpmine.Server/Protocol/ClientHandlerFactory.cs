using Microsoft.Extensions.Logging;

using Sharpmine.Server.Protocol.Packets;
using Sharpmine.Server.Security;

namespace Sharpmine.Server.Protocol;

public class ClientHandlerFactory(
    PacketDispatcher dispatcher,
    PlayerAccessManager playerAccessManager,
    ILoggerFactory loggerFactory
)
{

    public ClientHandler Create(
        string ip,
        TcpClient client,
        ServerService server)
    {
        var clientHandlerLogger = loggerFactory.CreateLogger<ClientHandler>();
        var packetLogger = loggerFactory.CreateLogger<PacketTransceiver>();
        var packetTransceiver = new PacketTransceiver(packetLogger);

        return new ClientHandler(
            ip,
            client,
            server,
            packetTransceiver,
            dispatcher,
            playerAccessManager,
            clientHandlerLogger);
    }

}
