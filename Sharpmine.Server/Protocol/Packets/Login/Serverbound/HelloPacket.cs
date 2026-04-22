using Optional;

using Sharpmine.Server.Protocol.DataTypes;
using Sharpmine.Server.Protocol.Packets.Login.Clientbound;

namespace Sharpmine.Server.Protocol.Packets.Login.Serverbound;

public partial record HelloPacket
{

    public string Name { get; set; } = null!;

    public Guid Uuid { get; set; }

    public ProtocolResult DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        Name = reader.ReadString();
        Uuid = new Guid(reader.ReadBytes(16));
        return ProtocolResult.Success;
    }

    public ValueTask<ProtocolResult> ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        // TODO: Replace dummy with actual GameProfile
        var profile = new GameProfile(
            Uuid,
            Name,
            [new GameProfileProperty("textures", "1337", Option.Some("Singapore"))]);

        handler.EnqueueClientboundPacket(new LoginFinishedPacket(profile));
        return ValueTask.FromResult(ProtocolResult.Success);
    }

}
