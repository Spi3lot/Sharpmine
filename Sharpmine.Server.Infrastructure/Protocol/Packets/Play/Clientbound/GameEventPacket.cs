using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.DataTypes;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Clientbound;

public partial record GameEventPacket
{

    private GameEventPacket(byte @event, float value = 0)
    {
        Event = @event;
        Value = value;
    }

    public byte Event { get; }

    public float Value { get; }

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteByte(Event);
        writer.WriteSingle(Value);
    }

    public static GameEventPacket NoRespawnBlockAvailable() => new(0);

    public static GameEventPacket BeginRaining() => new(1);

    public static GameEventPacket EndRaining() => new(2);

    public static GameEventPacket ChangeGameMode(GameMode gameMode) => new(3, (float) gameMode);

    public static GameEventPacket WinGame(bool credits) => new(4, (credits) ? 1 : 0);

    public static GameEventPacket Demo(DemoEvent demoEvent) => new(5, (float) demoEvent);

    public static GameEventPacket ArrowHitPlayer() => new(6);

    public static GameEventPacket RainLevelChange(float level) => new(7, level);

    public static GameEventPacket ThunderLevelChange(float level) => new(8, level);

    public static GameEventPacket PlayPufferfishStingSound() => new(9);

    public static GameEventPacket PlayerElderGuardianMobAppearance() => new(10);

    public static GameEventPacket EnableRespawnScreen(bool value) => new(11, (value) ? 1 : 0);

    public static GameEventPacket LimitedCrafting(bool value) => new(12, (value) ? 1 : 0);

    public static GameEventPacket StartWaitingForLevelChunks() => new(13);

}
