using System.Buffers;

using Sharpmine.Server.Protocol.Attributes;
using Sharpmine.Server.Protocol.Extensions;

namespace Sharpmine.Server.Protocol.Packets.Play.Serverbound;

public partial record AcceptTeleportationPacket
{

    [PacketProperty]
    private int _teleportId;

    public bool DeserializeContent(ref SequenceReader<byte> reader)
    {
        return reader.TryReadVarInt(out _teleportId);
    }

}
