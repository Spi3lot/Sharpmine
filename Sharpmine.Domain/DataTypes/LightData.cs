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

    private readonly List<byte[]> _skyLightArrays = [];

    private readonly List<byte[]> _blockLightArrays = [];

    public LightData(DimensionType dimensionType)
    {
        MaskBitCount = dimensionType.Height / 16 + 2; // + 1 below min height + 1 above max height
        _skyLightMask = new BitSet(MaskBitCount);
        _blockLightMask = new BitSet(MaskBitCount);
        _emptySkyLightMask = new BitSet(MaskBitCount);
        _emptyBlockLightMask = new BitSet(MaskBitCount);
        _emptySkyLightMask.SetAll(true);
        _emptyBlockLightMask.SetAll(true);
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

    public void SetBlockLight(int sectionIndex, byte[] lightData)
    {
        if (lightData.Length != 2048)
        {
            throw new ArgumentException("Minecraft light arrays must be exactly 2048 bytes.");
        }

        _blockLightMask[sectionIndex] = true;
        _emptyBlockLightMask[sectionIndex] = false;
        _blockLightArrays.Add(lightData);
    }

    public void ClearBlockLight(int sectionIndex)
    {
        _blockLightMask[sectionIndex] = false;
        _emptyBlockLightMask[sectionIndex] = true;
    }

    public void SetSkyLight(int sectionIndex, byte[] lightData)
    {
        if (lightData.Length != 2048)
        {
            throw new ArgumentException("Minecraft light arrays must be exactly 2048 bytes.");
        }

        _skyLightMask[sectionIndex] = true;
        _emptySkyLightMask[sectionIndex] = false;
        _skyLightArrays.Add(lightData);
    }

    public void ClearSkyLight(int sectionIndex)
    {
        _skyLightMask[sectionIndex] = false;
        _emptySkyLightMask[sectionIndex] = true;
    }

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.Write(_skyLightMask);
        writer.Write(_blockLightMask);
        writer.Write(_emptySkyLightMask);
        writer.Write(_emptyBlockLightMask);
        writer.WritePrefixedArray(_skyLightArrays.ToArray(), static (writer, skyLightArray) => writer.WritePrefixed(skyLightArray));
        writer.WritePrefixedArray(_blockLightArrays.ToArray(), static (writer, blockLightArray) => writer.WritePrefixed(blockLightArray));
    }

}
