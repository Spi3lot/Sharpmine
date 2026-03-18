using System.Net.Sockets;
using Sharpmine.Server.Protocol.Packets.Status.Clientbound;

namespace Sharpmine.Server.Protocol.Packets.Status.Serverbound;

public partial record PingRequestPacket
{

    public long Timestamp { get; set; }

    public Task DeserializeContentAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken)
    {
        // TODO: Maybe catch exceptions and return Task.FromException
        Timestamp = reader.Read7BitEncodedInt64();
        return Task.CompletedTask;
    }

    public async Task ProcessAsync(ClientHandler handler, NetworkStream stream, BinaryReader reader, BinaryWriter writer, CancellationToken cancellationToken)
    {
        var response = new PongResponsePacket(Timestamp);
        await handler.PacketSender.SendAsync(response, stream, writer, cancellationToken);
        handler.Client.Close();
    }

}