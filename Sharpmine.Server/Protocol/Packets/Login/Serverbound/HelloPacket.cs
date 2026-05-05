using Optional;

using Sharpmine.Server.Protocol.DataTypes;
using Sharpmine.Server.Protocol.Packets.Login.Clientbound;
using Sharpmine.Server.Security;

namespace Sharpmine.Server.Protocol.Packets.Login.Serverbound;

public partial record HelloPacket
{

    public string Name { get; set; } = null!;

    public Guid Uuid { get; set; }

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        Name = reader.ReadString();
        Uuid = new Guid(reader.ReadBytes(16));
        return true;
    }

    public async ValueTask ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        var (access, reason) = handler.AccessManager.EvaluateAccess(handler.Ip, Uuid);

        if (access is not JoinAccess.Allowed)
        {
            await handler.DisconnectAsync(reason!);
            return;
        }

        if (!handler.CapacityManager.TryReserveSlot(handler.AccessManager.BypassesPlayerLimit(Uuid)))
        {
            await handler.DisconnectAsync("Server is full!");
            return;
        }

        // TODO: Send empty .Properties (only in offline-mode)
        var profile = new GameProfile(
            Uuid,
            Name,
            [new GameProfileProperty("textures", "1337", Option.Some("Singapore"))]);

        handler.SendPacket(new LoginFinishedPacket(profile));
    }

}
