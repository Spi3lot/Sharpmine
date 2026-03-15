using System.Net.Sockets;

namespace Sharpmine.Server.Protocol.Packets.Play.Serverbound;

public partial record AcceptTeleportationPacket
{

    public int TeleportId { get; set; }

    public Task DeserializeContentAsync(NetworkStream stream, BinaryReader reader)
    {
        TeleportId = reader.Read7BitEncodedInt();
        return Task.CompletedTask;
    }

    public Task ProcessAsync(ClientHandler handler, NetworkStream stream, BinaryReader reader, BinaryWriter writer)
    {
        throw new NotImplementedException();
    }

}