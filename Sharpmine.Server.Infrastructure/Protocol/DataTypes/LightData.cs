using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public class LightData : IClientboundDataType
{

    public const int MaskBitCount = (64 + 320) / 16 + 2; // 24 + 2 (1 below min height + 1 above max height)

    public BitSet SkyLightMask { get; set; } = new(MaskBitCount);

    public BitSet BlockLightMask { get; set; } = new(MaskBitCount);

    public BitSet EmptySkyLightMask { get; set; } = new(MaskBitCount);

    public BitSet EmptyBlockLightMask { get; set; } = new(MaskBitCount);

    public byte[][] SkyLightArrays { get; set; } = [];

    public byte[][] BlockLightArrays { get; set; } = [];

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.Write(SkyLightMask);
        writer.Write(BlockLightMask);
        writer.Write(EmptySkyLightMask);
        writer.Write(EmptyBlockLightMask);
        writer.WritePrefixedArray(SkyLightArrays, static (writer, skyLightArray) => writer.WritePrefixed(skyLightArray));
        writer.WritePrefixedArray(BlockLightArrays, static (writer, blockLightArray) => writer.WritePrefixed(blockLightArray));
    }

}
