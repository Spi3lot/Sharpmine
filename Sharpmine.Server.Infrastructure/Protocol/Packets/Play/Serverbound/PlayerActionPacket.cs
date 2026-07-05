using System.Buffers;

using Sharpmine.Domain.DataTypes;
using Sharpmine.Domain.Extensions;
using Sharpmine.Server.Infrastructure.Protocol.Attributes;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Serverbound;

public partial record PlayerActionPacket
{

    [PacketProperty]
    private PlayerActionStatus _status;

    [PacketProperty]
    private Position _location;

    [PacketProperty]
    private PlayerActionFace _face;

    [PacketProperty]
    private int _sequence;

    public bool DeserializeContent(ref SequenceReader<byte> reader)
    {
        return reader.TryReadVarIntEnum(out _status)
               && reader.TryRead(out _location)
               && reader.TryReadEnum<PlayerActionFace, sbyte>(out _face, static (ref reader, out value) => reader.TryReadSByte(out value))
               && reader.TryReadVarInt(out _sequence);
    }

}
