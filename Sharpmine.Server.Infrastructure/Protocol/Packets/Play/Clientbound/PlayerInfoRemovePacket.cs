using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Clientbound;

public partial record PlayerInfoRemovePacket(Guid[] Uuids)
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WritePrefixedArray(Uuids, static (writer, uuid) => writer.WriteUuid(uuid));
    }

}
