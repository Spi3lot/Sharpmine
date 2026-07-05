using System.Buffers;

using Sharpmine.Domain.DataTypes;
using Sharpmine.Domain.Extensions;
using Sharpmine.Server.Infrastructure.Protocol.Attributes;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Serverbound;

public partial record ChangeGameModePacket
{

    [PacketProperty]
    private GameMode _gameMode;

    public bool DeserializeContent(ref SequenceReader<byte> reader)
    {
        return reader.TryReadVarIntEnum(out _gameMode);
    }

}
