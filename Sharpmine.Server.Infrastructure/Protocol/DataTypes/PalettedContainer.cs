using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public sealed class PalettedContainer : IClientboundDataType
{

    private readonly int _capacity;

    private readonly int _yShift;

    private readonly int _zShift;

    private readonly byte _minIndirectBpe;

    private readonly byte _maxIndirectBpe;

    private readonly byte _directBpe;

    private byte _bitsPerEntry;

    private int[] _palette = [0];

    private long[] _data = [];

    public PalettedContainer(PalettedContainerType type)
    {
        bool biomes = type == PalettedContainerType.Biomes;
        _capacity = (biomes) ? 64 : 4096;
        _yShift = (biomes) ? 4 : 8;
        _zShift = (biomes) ? 2 : 4;
        _minIndirectBpe = (byte) ((biomes) ? 1 : 4);
        _maxIndirectBpe = (byte) ((biomes) ? 3 : 8);
        _directBpe = (byte) ((biomes) ? 7 : 15);
    }

    public int Get(int x, int y, int z)
    {
        int index = GetLinearIndex(x, y, z);
        return GetStateAt(index, _bitsPerEntry, _directBpe, _palette, _data);
    }

    public void Set(int x, int y, int z, int stateId)
    {
        if (IsSingleValued(_bitsPerEntry))
        {
            if (_palette[0] == stateId) return;
            Resize(_minIndirectBpe);
        }

        int paletteIndex;

        if (IsDirect(_bitsPerEntry, _directBpe))
        {
            paletteIndex = stateId;
        }
        else
        {
            paletteIndex = Array.IndexOf(_palette, stateId);
            if (paletteIndex == -1) paletteIndex = AddToPaletteOrResize(stateId);
        }

        SetDataAt(GetLinearIndex(x, y, z), paletteIndex, _bitsPerEntry, _data);
    }

    private int AddToPaletteOrResize(int stateId)
    {
        if (_palette.Length < (1 << _bitsPerEntry))
        {
            int index = _palette.Length;
            Array.Resize(ref _palette, _palette.Length + 1);
            _palette[index] = stateId;
            return index;
        }

        byte nextIndirectBpe = (byte) (_bitsPerEntry + 1);
        if (nextIndirectBpe > _maxIndirectBpe)
        {
            Resize(_directBpe);
            return stateId;
        }

        Resize(nextIndirectBpe);
        int newIndex = _palette.Length;
        Array.Resize(ref _palette, _palette.Length + 1);
        _palette[newIndex] = stateId;
        return newIndex;
    }

    private void Resize(byte newBpe)
    {
        int[] oldPalette = _palette;
        long[] oldData = _data;
        byte oldBpe = _bitsPerEntry;
        _bitsPerEntry = newBpe;

        if (IsDirect(_bitsPerEntry, _directBpe))
        {
            _palette = [];
        }

        int entriesPerLong = 64 / _bitsPerEntry;
        int longsCount = (_capacity + entriesPerLong - 1) / entriesPerLong;
        _data = new long[longsCount];

        for (int i = 0; i < _capacity; i++)
        {
            int stateId = GetStateAt(i, oldBpe, _directBpe, oldPalette, oldData);
            int newPaletteIndex = (IsDirect(_bitsPerEntry, _directBpe)) ? stateId : Array.IndexOf(_palette, stateId);
            SetDataAt(i, newPaletteIndex, _bitsPerEntry, _data);
        }
    }

    private int GetLinearIndex(int x, int y, int z) => (y << _yShift) | (z << _zShift) | x;

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteByte(_bitsPerEntry);

        if (IsSingleValued(_bitsPerEntry))
        {
            writer.WriteVarInt(_palette[0]);
        }
        else if (IsDirect(_bitsPerEntry, _directBpe))
        {
            writer.WriteArray(_data, static (writer, @long) => writer.WriteInt64(@long));
        }
        else
        {
            writer.WritePrefixedArray(_palette, static (writer, @long) => writer.WriteVarInt(@long));
            writer.WriteArray(_data, static (writer, @long) => writer.WriteInt64(@long));
        }
    }

    private static int GetStateAt(int index, byte bpe, byte directBpe, int[] palette, long[] data)
    {
        if (IsSingleValued(bpe))
        {
            return palette[0];
        }

        int entriesPerLong = 64 / bpe;
        int longIndex = index / entriesPerLong;
        int bitIndex = (index % entriesPerLong) * bpe;
        long entryMask = (1L << bpe) - 1;
        int paletteIndex = (int) ((data[longIndex] >>> bitIndex) & entryMask);
        return (IsDirect(bpe, directBpe)) ? paletteIndex : palette[paletteIndex];
    }

    private static void SetDataAt(int index, int paletteIndex, byte bpe, long[] data)
    {
        if (IsSingleValued(bpe))
        {
            return;
        }

        int entriesPerLong = 64 / bpe;
        int longIndex = index / entriesPerLong;
        int bitIndex = (index % entriesPerLong) * bpe;
        long entryMask = (1L << bpe) - 1;
        data[longIndex] &= ~(entryMask << bitIndex);
        data[longIndex] |= (paletteIndex & entryMask) << bitIndex;
    }

    private static bool IsSingleValued(byte bitsPerEntry) => bitsPerEntry == 0;

    private static bool IsDirect(byte bitsPerEntry, byte directBpe) => bitsPerEntry >= directBpe;

}
