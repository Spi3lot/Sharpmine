using System.Buffers;
using System.Collections;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public class LightData : IClientboundDataType
{

    public BitArray SkyLightMask { get; set; } = new(0);

    public BitArray BlockLightMask { get; set; } = new(0);

    public BitArray EmptySkyLightMask { get; set; } = new(0);

    public BitArray EmptyBlockLightMask { get; set; } = new(0);

    public byte[][] SkyLightArrays { get; set; } = [];

    public byte[][] BlockLightArrays { get; set; } = [];

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.Write(SkyLightMask);
        writer.Write(BlockLightMask);
        writer.Write(EmptySkyLightMask);
        writer.Write(EmptyBlockLightMask);
        writer.WritePrefixedArray(SkyLightArrays, static (writer, skyLightArray) => writer.WritePrefixedArray(skyLightArray, static (writer, skyLight) => writer.WriteByte(skyLight)));
        writer.WritePrefixedArray(BlockLightArrays, static (writer, blockLightArray) => writer.WritePrefixedArray(blockLightArray, static (writer, blockLight) => writer.WriteByte(blockLight)));
    }

}
