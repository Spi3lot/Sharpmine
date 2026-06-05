using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Attributes;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Serverbound;

public partial record MovePlayerPosRotPacket
{

    [PacketProperty]
    private double _x;

    [PacketProperty]
    private double _feetY;

    [PacketProperty]
    private double _z;

    [PacketProperty]
    private float _yaw;

    [PacketProperty]
    private float _pitch;

    [PacketProperty]
    private MovementStates _flags;

    public bool DeserializeContent(ref SequenceReader<byte> reader)
    {
        return reader.TryReadDouble(out _x)
               && reader.TryReadDouble(out _feetY)
               && reader.TryReadDouble(out _z)
               && reader.TryReadSingle(out _yaw)
               && reader.TryReadSingle(out _pitch)
               && reader.TryReadEnum<MovementStates, byte>(out _flags, static (ref reader, out value) => reader.TryReadByte(out value));
    }

}
