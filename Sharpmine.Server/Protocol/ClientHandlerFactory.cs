using System.Net.Sockets;

using Microsoft.Extensions.Logging;

using Sharpmine.Server.Protocol.Packets;

namespace Sharpmine.Server.Protocol;

public class ClientHandlerFactory(ILoggerFactory loggerFactory)
{

    public ClientHandler Create(TcpClient client)
    {
        var clientHandlerLogger = loggerFactory.CreateLogger<ClientHandler>();
        var packetSenderLogger = loggerFactory.CreateLogger<PacketSender>();
        var packetSender = new PacketSender(packetSenderLogger);
        return new ClientHandler(client, packetSender, clientHandlerLogger);
    }

}