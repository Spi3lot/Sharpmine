using System.Net.Sockets;

namespace Sharpmine.Server.Protocol.Packets.Configuration.Serverbound;

public partial record CustomPayloadPacket
{

    public string Channel { get; set; } = null!;
    
    // TODO: Data

    public Task DeserializeContentAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken)
    {
        Channel = reader.ReadString();
        return Task.CompletedTask;
    }

    public Task ProcessAsync(
        ClientHandler handler,
        NetworkStream stream,
        BinaryReader reader,
        BinaryWriter writer,
        CancellationToken cancellationToken) => Task.CompletedTask;

}
