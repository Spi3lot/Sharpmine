using System.Buffers;

using Sharpmine.Domain.Extensions;
using Sharpmine.Server.Infrastructure.Protocol.Attributes;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Serverbound;

public partial record MovePlayerPosPacket
{

    [PacketProperty]
    private double _x;

    [PacketProperty]
    private double _feetY;

    [PacketProperty]
    private double _z;

    [PacketProperty]
    private MovementStates _flags;

    public bool DeserializeContent(ref SequenceReader<byte> reader)
    {
        return reader.TryReadDouble(out _x)
               && reader.TryReadDouble(out _feetY)
               && reader.TryReadDouble(out _z)
               && reader.TryReadEnum<MovementStates, byte>(out _flags, static (ref reader, out value) => reader.TryReadByte(out value));
    }

}
