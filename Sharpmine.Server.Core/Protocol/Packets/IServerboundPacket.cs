using System.Buffers;

namespace Sharpmine.Server.Core.Protocol.Packets;

public interface IServerboundPacket : IPacket
{

    bool DeserializeContent(ref SequenceReader<byte> reader)
        => throw new NotImplementedException();

    string ToString();

}
