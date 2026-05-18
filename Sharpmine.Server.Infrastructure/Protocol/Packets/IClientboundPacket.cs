using System.Buffers;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets;

public interface IClientboundPacket : IPacket
{

    void SerializeContent(IBufferWriter<byte> writer)
        => throw new NotImplementedException();

    string ToString();

}
