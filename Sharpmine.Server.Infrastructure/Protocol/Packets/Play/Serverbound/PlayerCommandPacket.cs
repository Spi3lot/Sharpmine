using System.Buffers;

using Sharpmine.Domain.DataTypes;
using Sharpmine.Domain.Extensions;
using Sharpmine.Server.Infrastructure.Protocol.Attributes;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Serverbound;

public partial record PlayerCommandPacket
{

    [PacketProperty]
    private int _entityId;

    [PacketProperty]
    private PlayerCommandAction _action;

    [PacketProperty]
    private int _jumpBoost;

    public bool DeserializeContent(ref SequenceReader<byte> reader)
    {
        return reader.TryReadVarInt(out _entityId)
               && reader.TryReadVarIntEnum(out _action)
               && reader.TryReadVarInt(out _jumpBoost);
    }

}
