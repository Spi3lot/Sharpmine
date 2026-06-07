using System.Buffers;

using Optional;

using Sharpmine.Domain.DataTypes;
using Sharpmine.Server.Infrastructure.Protocol.DataTypes;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Clientbound;

public partial record LoginPacket(
    int EntityId,
    bool IsHardcore,
    string[] DimensionNames,
    int MaxPlayers,
    int ViewDistance,
    int SimulationDistance,
    bool ReducedDebugInfo,
    bool EnableRespawnScreen,
    bool DoLimitedCrafting,
    int DimensionType,
    string DimensionName,
    long HashedSeed,
    GameMode GameMode,
    GameMode PreviousGameMode,
    bool IsDebug,
    bool IsFlat,
    bool HasDeathLocation,
    Option<string> DeathDimensionName,
    Option<Position> DeathLocation,
    int PortalCooldown,
    int SeaLevel,
    bool EnforcesSecureChat)
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteInt32(EntityId);
        writer.WriteBoolean(IsHardcore);
        writer.WritePrefixedArray(DimensionNames, static (writer, dimensionName) => writer.WriteString(dimensionName));
        writer.WriteVarInt(MaxPlayers);
        writer.WriteVarInt(ViewDistance);
        writer.WriteVarInt(SimulationDistance);
        writer.WriteBoolean(ReducedDebugInfo);
        writer.WriteBoolean(EnableRespawnScreen);
        writer.WriteBoolean(DoLimitedCrafting);
        writer.WriteVarInt(DimensionType);
        writer.WriteString(DimensionName);
        writer.WriteInt64(HashedSeed);
        writer.WriteByte((byte) GameMode);
        writer.WriteSByte((sbyte) PreviousGameMode);
        writer.WriteBoolean(IsDebug);
        writer.WriteBoolean(IsFlat);
        writer.WriteBoolean(HasDeathLocation);
        writer.WriteOptional(DeathDimensionName, static (writer, dimensionName) => writer.WriteString(dimensionName));
        writer.WriteOptional(DeathLocation);
        writer.WriteVarInt(PortalCooldown);
        writer.WriteVarInt(SeaLevel);
        writer.WriteBoolean(EnforcesSecureChat);
    }

}
