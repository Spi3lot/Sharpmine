using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;
using Sharpmine.Server.Infrastructure.Protocol.Attributes;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Serverbound;

public partial record AcceptTeleportationPacket
{

    [PacketProperty]
    private int _teleportId;

    public bool DeserializeContent(ref SequenceReader<byte> reader)
    {
        return reader.TryReadVarInt(out _teleportId);
    }

}
