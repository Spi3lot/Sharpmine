using System.Buffers;

namespace Sharpmine.Server.Core.Protocol.DataTypes;

public interface IClientboundDataType
{

    void Serialize(IBufferWriter<byte> writer);

}