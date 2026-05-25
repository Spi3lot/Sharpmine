using Optional;

using Sharpmine.Server.Infrastructure.Configuration;
using Sharpmine.Server.Infrastructure.Protocol.DataTypes;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Configuration.Serverbound;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Clientbound;

namespace Sharpmine.Server.Infrastructure.Protocol.Handlers.Configuration;

public class FinishConfigurationPacketHandler(ServerProperties properties) : IPacketHandler<FinishConfigurationPacket>
{

    public ValueTask HandleAsync(
        FinishConfigurationPacket packet,
        ClientHandler client,
        CancellationToken cancellationToken)
    {
        var loginPacket = new LoginPacket(
            EntityId: 0,
            IsHardcore: false,
            DimensionNames: ["minecraft:overworld"],
            MaxPlayers: properties.MaxPlayers,
            ViewDistance: 16,
            SimulationDistance: 16,
            ReducedDebugInfo: false,
            EnableRespawnScreen: true,
            DoLimitedCrafting: false,
            DimensionType: 0,
            DimensionName: "minecraft:overworld",
            HashedSeed: 0000,
            GameMode: GameMode.Creative,
            PreviousGameMode: GameMode.Undefined,
            IsDebug: false,
            IsFlat: true,
            HasDeathLocation: false,
            DeathDimensionName: Option.None<string>(),
            DeathLocation: Option.None<Position>(),
            PortalCooldown: 0,
            SeaLevel: 63,
            EnforcesSecureChat: false);

        client.SendPacket(loginPacket);
        return ValueTask.CompletedTask;
    }

}
