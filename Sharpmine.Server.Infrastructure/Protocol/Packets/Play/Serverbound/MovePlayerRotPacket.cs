using System.Buffers;

using Sharpmine.Domain.Extensions;
using Sharpmine.Server.Infrastructure.Protocol.Attributes;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Serverbound;

public partial record MovePlayerRotPacket
{

    [PacketProperty]
    private float _yaw;

    [PacketProperty]
    private float _pitch;

    [PacketProperty]
    private MovementStates _flags;

    public bool DeserializeContent(ref SequenceReader<byte> reader)
    {
        return reader.TryReadSingle(out _yaw)
               && reader.TryReadSingle(out _pitch)
               && reader.TryReadEnum<MovementStates, byte>(out _flags, static (ref reader, out value) => reader.TryReadByte(out value));
    }

}
