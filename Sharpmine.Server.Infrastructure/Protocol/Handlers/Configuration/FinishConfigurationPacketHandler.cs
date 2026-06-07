using Optional;

using Sharpmine.Domain.DataTypes;
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
        client.SendPacket(new LoginPacket(
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
            EnforcesSecureChat: false));

        client.SendPacket(new PlayerPositionPacket(
            TeleportId: 1,
            X: 0,
            Y: 100,
            Z: 0,
            VelocityX: 0,
            VelocityY: 0,
            VelocityZ: 0,
            Yaw: 0,
            Pitch: 0,
            Flags: TeleportRelativeAxes.None
        ));

        // TODO: client.SendPacket(new PlayerInfoUpdatePacket());
        client.SendPacket(GameEventPacket.StartWaitingForLevelChunks());

        client.SendPacket(new SetChunkCacheCenterPacket(0, 0));
        return ValueTask.CompletedTask;
    }

}
