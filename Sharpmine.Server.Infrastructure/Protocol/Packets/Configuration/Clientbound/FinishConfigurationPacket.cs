using System.Buffers;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Configuration.Clientbound;

public partial record FinishConfigurationPacket
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
    }

}
