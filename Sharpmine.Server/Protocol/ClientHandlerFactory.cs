using Microsoft.Extensions.Logging;

using Sharpmine.Server.Protocol.Packets;
using Sharpmine.Server.Security;

namespace Sharpmine.Server.Protocol;

public class ClientHandlerFactory(ILoggerFactory loggerFactory)
{

    public ClientHandler Create(
        TcpClient client,
        ServerService server,
        PlayerAccessManager playerAccessManager)
    {
        var clientHandlerLogger = loggerFactory.CreateLogger<ClientHandler>();
        var packetLogger = loggerFactory.CreateLogger<PacketTransceiver>();
        var packetTransceiver = new PacketTransceiver(packetLogger);
        var handler = new ClientHandler(client, server, packetTransceiver, playerAccessManager, clientHandlerLogger);
        return handler;
    }

}
