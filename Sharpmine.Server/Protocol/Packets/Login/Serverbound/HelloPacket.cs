using System.Net.Sockets;

using Sharpmine.Server.Protocol.Packets.Login.Clientbound;

namespace Sharpmine.Server.Protocol.Packets.Login.Serverbound;

public partial record HelloPacket
{

    public string Name { get; set; }

    public Guid Uuid { get; set; }

    public Task DeserializeContentAsync(NetworkStream stream, BinaryReader reader)
    {
        Name = reader.ReadString();
        Uuid = new Guid(reader.ReadBytes(16));
        return Task.CompletedTask;
    }

    public Task ProcessAsync(ClientHandler handler, NetworkStream stream, BinaryReader reader, BinaryWriter writer)
    {
        var profile = new GameProfile
        {
            Username = Name,
            Uuid = Uuid,
            Properties = new GameProfileProperties
            {
                Name = Name,
                Value = "1337",
                Signature = "Singapore",
            }
        };

        var packet = new LoginFinishedPacket(in profile);
        return handler.PacketSender.SendAsync(packet, stream, writer);
    }

}
