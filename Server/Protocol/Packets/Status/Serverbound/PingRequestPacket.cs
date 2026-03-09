using System.Net.Sockets;
using Sharpmine.Server.Protocol.Packets.Status.Clientbound;

namespace Sharpmine.Server.Protocol.Packets.Status.Serverbound;

public partial class PingRequestPacket : IServerboundPacket
{

    public long Timestamp { get; set; }

    public Task DeserializeAsync(BinaryReader reader)
    {
        // TODO: Maybe catch exceptions and return Task.FromException
        Timestamp = reader.Read7BitEncodedInt64();
        return Task.CompletedTask;
    }

    public async Task ProcessAsync(ClientHandler handler, NetworkStream stream, BinaryReader reader, BinaryWriter writer)
    {
        var response = new PongResponsePacket { Timestamp = Timestamp };
        await handler.PacketSender.SendAsync(response, stream, writer);
        handler.Client.Close();
    }

}