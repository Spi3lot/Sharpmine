using System.Buffers;

using Sharpmine.Domain.Extensions;
using Sharpmine.Server.Infrastructure.Protocol.Attributes;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Serverbound;

public partial record MovePlayerStatusOnlyPacket
{

    [PacketProperty]
    private MovementStates _flags;

    public bool DeserializeContent(ref SequenceReader<byte> reader)
    {
        return reader.TryReadEnum<MovementStates, sbyte>(out _flags, static (ref reader, out value) => reader.TryReadSByte(out value));
    }

}
