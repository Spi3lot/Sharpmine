using Optional;

using Sharpmine.Server.Protocol.DataTypes;
using Sharpmine.Server.Protocol.Packets.Login.Clientbound;

namespace Sharpmine.Server.Protocol.Packets.Login.Serverbound;

public partial record HelloPacket
{

    public string Name { get; set; } = null!;

    public Guid Uuid { get; set; }

    public static Task DeserializeAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken)
    {
        Name = reader.ReadString();
        Uuid = new Guid(reader.ReadBytes(16));
        return Task.CompletedTask;
    }

    public Task ProcessAsync(
        ClientHandler handler,
        NetworkStream stream,
        BinaryReader reader,
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        // TODO
        var profile = new GameProfile(
            Uuid,
            Name,
            [new GameProfileProperty("textures", "1337", Option.Some("Singapore"))]);

        var packet = new LoginFinishedPacket(profile);
        return handler.PacketTransceiver.TransmitAsync(packet, stream, writer, cancellationToken);
    }

}
