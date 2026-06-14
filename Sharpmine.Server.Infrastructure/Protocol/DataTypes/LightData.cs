using System.Buffers;

using Sharpmine.Domain.Registries.Dynamic.Entries;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public class LightData : IClientboundDataType
{

    public LightData(DimensionType dimensionType)
    {
        MaskBitCount = dimensionType.Height / 16 + 2; // + 1 below min height + 1 above max height
        SkyLightMask = new BitSet(MaskBitCount);
        BlockLightMask = new BitSet(MaskBitCount);
        EmptySkyLightMask = new BitSet(MaskBitCount);
        EmptyBlockLightMask = new BitSet(MaskBitCount);
    }

    public int MaskBitCount { get; }

    public BitSet SkyLightMask { get; }

    public BitSet BlockLightMask { get; }

    public BitSet EmptySkyLightMask { get; }

    public BitSet EmptyBlockLightMask { get; }

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
