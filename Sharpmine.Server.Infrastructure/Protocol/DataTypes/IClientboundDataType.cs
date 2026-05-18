using System.Buffers;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public interface IClientboundDataType
{

    void Serialize(IBufferWriter<byte> writer);

}