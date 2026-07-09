using System.Buffers;

using Sharpmine.Domain.Extensions;
using Sharpmine.Domain.Registries.Dynamic.Entries;

namespace Sharpmine.Domain.DataTypes;

public class LightData : IClientboundDataType
{

    private readonly BitSet _skyLightMask;

    private readonly BitSet _blockLightMask;

    private readonly BitSet _emptySkyLightMask;

    private readonly BitSet _emptyBlockLightMask;

    private readonly byte[]?[] _skyLightArrays;

    private readonly byte[]?[] _blockLightArrays;

    public LightData(DimensionType dimensionType)
    {
        MaskBitCount = dimensionType.Height / 16 + 2; // + 1 below min height + 1 above max height
        _skyLightMask = new BitSet(MaskBitCount);
        _blockLightMask = new BitSet(MaskBitCount);
        _emptySkyLightMask = new BitSet(MaskBitCount);
        _emptyBlockLightMask = new BitSet(MaskBitCount);
        _emptySkyLightMask.SetAll(true);
        _emptyBlockLightMask.SetAll(true);

        _skyLightArrays = new byte[]?[MaskBitCount];
        _blockLightArrays = new byte[]?[MaskBitCount];
    }

    public int MaskBitCount { get; }

    public static void SetLightLevel(byte[] lightData, int x, int y, int z, byte lightLevel)
    {
        int blockIndex = (y << 8) | (z << 4) | x;
        int byteIndex = blockIndex / 2;

        lightData[byteIndex] = (blockIndex % 2 == 0)
            ? (byte) ((lightData[byteIndex] & 0xF0) | (lightLevel & 0x0F))
            : (byte) ((lightData[byteIndex] & 0x0F) | ((lightLevel & 0x0F) << 4));
    }

    public void SetSkyLight(int sectionY, byte[]? lightData)
    {
        if (lightData is { Length: not 2048 })
        {
            throw new ArgumentException("Minecraft light arrays must be exactly 2048 bytes.");
        }

        int bitIndex = sectionY + 1;
        _skyLightMask[bitIndex] = lightData is not null;
        _emptySkyLightMask[bitIndex] = lightData is null;
        _skyLightArrays[bitIndex] = lightData;
    }

    public void SetBlockLight(int sectionY, byte[]? lightData)
    {
        if (lightData is { Length: not 2048 })
        {
            throw new ArgumentException("Minecraft light arrays must be exactly 2048 bytes.");
        }

        int bitIndex = sectionY + 1;
        _blockLightMask[bitIndex] = lightData is not null;
        _emptyBlockLightMask[bitIndex] = lightData is null;
        _blockLightArrays[bitIndex] = lightData;
    }

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.Write(_skyLightMask);
        writer.Write(_blockLightMask);
        writer.Write(_emptySkyLightMask);
        writer.Write(_emptyBlockLightMask);

        var activeSkyLights = _skyLightArrays.Where(x => x is not null);
        var activeBlockLights = _blockLightArrays.Where(x => x is not null);
        writer.WritePrefixedEnumerable(activeSkyLights, static (writer, arr) => writer.WritePrefixed(arr));
        writer.WritePrefixedEnumerable(activeBlockLights, static (writer, arr) => writer.WritePrefixed(arr));
    }

}
