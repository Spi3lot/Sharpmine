using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;
using Sharpmine.Server.Infrastructure.Protocol.Attributes;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Login.Serverbound;

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
