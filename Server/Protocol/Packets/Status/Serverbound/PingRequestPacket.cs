using System.Net.Sockets;
using Sharpmine.Server.Protocol.Packets.Status.Clientbound;

namespace Sharpmine.Server.Protocol.Packets.Status.Serverbound;

[Packet(0x01, ConnectionState.Status)]
public class PingRequestPacket : IServerboundPacket
{

    public long Timestamp { get; set; }

    public Task DeserializeAsync(BinaryReader reader) => Task.CompletedTask;

    public Task ProcessAsync(ClientHandler handler, NetworkStream stream, BinaryReader reader, BinaryWriter writer)
    {
        var response = new PongResponsePacket { Timestamp = Timestamp };
        return handler.PacketSender.SendAsync(response, stream, writer);
    }

}