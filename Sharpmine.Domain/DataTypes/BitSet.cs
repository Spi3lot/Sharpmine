using System.Buffers;

using Sharpmine.Domain.Extensions;

namespace Sharpmine.Domain.DataTypes;

public readonly struct BitSet(int bitCapacity) : IClientboundDataType
{

    public int BitCapacity { get; } = bitCapacity;

    public long[] Data { get; } = new long[(bitCapacity + 63) / 64];

    public bool this[int index]
    {
        get => (Data[index / 64] & (1L << (index % 64))) != 0;
        set
        {
            if (value) Data[index / 64] |= 1L << (index % 64);
            else Data[index / 64] &= ~(1L << (index % 64));
        }
    }

    public void SetAll(bool value)
    {
        Array.Fill(Data, (value) ? ~0 : 0);
    }

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WritePrefixedArray(Data, static (writer, @long) => writer.WriteInt64(@long));
    }

}
