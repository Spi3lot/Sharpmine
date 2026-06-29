using System.Buffers;

namespace Sharpmine.Domain.DataTypes;

public interface IClientboundDataType
{

    void Serialize(IBufferWriter<byte> writer);

}
