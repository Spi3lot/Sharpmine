using System.Buffers;

using Sharpmine.Server.Core.Protocol.Attributes;
using Sharpmine.Server.Core.Protocol.Extensions;

namespace Sharpmine.Server.Core.Protocol.Packets.Play.Serverbound;

public partial record AcceptTeleportationPacket
{

    [PacketProperty]
    private int _teleportId;

    public bool DeserializeContent(ref SequenceReader<byte> reader)
    {
        return reader.TryReadVarInt(out _teleportId);
    }

}
