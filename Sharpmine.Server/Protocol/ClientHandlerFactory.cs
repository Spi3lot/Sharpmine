using Microsoft.Extensions.Logging;

using Sharpmine.Server.Protocol.Packets;

namespace Sharpmine.Server.Protocol;

public class ClientHandlerFactory(ILoggerFactory loggerFactory)
{

    public ClientHandler Create(TcpClient client, ServerService server)
    {
        var clientHandlerLogger = loggerFactory.CreateLogger<ClientHandler>();
        var packetLogger = loggerFactory.CreateLogger<PacketTransceiver>();
        var packetTransceiver = new PacketTransceiver(packetLogger);
        var handler = new ClientHandler(client, server, clientHandlerLogger);
        packetTransceiver.Handler = handler;
        handler.PacketTransceiver = packetTransceiver;
        return handler;
    }

}
