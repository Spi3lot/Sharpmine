using Optional;

using Sharpmine.Domain;
using Sharpmine.Domain.DataTypes;
using Sharpmine.Domain.Registries.Static;
using Sharpmine.Server.Domain.Registries.Dynamic;
using Sharpmine.Server.Infrastructure.Configuration;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Configuration.Serverbound;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Clientbound;

namespace Sharpmine.Server.Infrastructure.Protocol.Handlers.Configuration;

public class FinishConfigurationPacketHandler(
    ServerProperties properties,
    IRegistries registries) : IPacketHandler<FinishConfigurationPacket>
{

    public ValueTask HandleAsync(
        FinishConfigurationPacket packet,
        ClientHandler client,
        CancellationToken cancellationToken)
    {
        var player = client.Player!;
        var overworldId = Identifier.Minecraft("overworld");

        client.SendPacket(new LoginPacket(
            EntityId: 0,
            IsHardcore: false,
            DimensionNames: [.. registries.DimensionTypes.Keys],
            MaxPlayers: properties.MaxPlayers,
            ViewDistance: properties.ViewDistance,
            SimulationDistance: properties.SimulationDistance,
            ReducedDebugInfo: false,
            EnableRespawnScreen: true,
            DoLimitedCrafting: false,
            DimensionType: 0,
            DimensionName: overworldId,
            HashedSeed: 0000,
            GameMode: player.GameMode,
            PreviousGameMode: player.PreviousGameMode,
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

        var entry = new PlayerInfoEntry(
            Uuid: player.Profile.Uuid,
            Name: player.Profile.Username,
            Properties: player.Profile.Properties
            // TODO: More actions
        );

        var overworld = registries.DimensionTypes[overworldId];
        var chunk = new Chunk(overworld);
        chunk.SetBlock(0, 0, 0, Blocks.AcaciaLog.DefaultState.Id);

        // TODO: Abstract away
        var lightData = new LightData(overworld);
        var lightBytes = new byte[2048];
        LightData.SetLightLevel(lightBytes, 0, 0, 0, 15);
        lightData.SetBlockLight(0, lightBytes);
        lightData.SetSkyLight(0, lightBytes);

        client.SendPacket(new PlayerInfoUpdatePacket(PlayerActions.AddPlayer, [entry]));
        client.SendPacket(GameEventPacket.StartWaitingForLevelChunks);
        client.SendPacket(new SetChunkCacheCenterPacket(0, 0)); // TODO: Send on every chunk border crossing
        client.SendPacket(new LevelChunkWithLightPacket(0, 0, chunk, lightData));
        return ValueTask.CompletedTask;
    }

}
