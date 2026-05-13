using System.Buffers;

using Sharpmine.Server.Core.Protocol.Attributes;
using Sharpmine.Server.Core.Protocol.Extensions;

namespace Sharpmine.Server.Core.Protocol.Packets.Login.Serverbound;

public partial record HelloPacket
{

    [PacketProperty]
    private string _name;

    [PacketProperty]
    private Guid _uuid;

    public bool DeserializeContent(ref SequenceReader<byte> reader)
    {
        return reader.TryReadString(out _name, 16)
               && reader.TryReadUuid(out _uuid);
    }

}
