using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public readonly struct BitSet(int bitCapacity) : IClientboundDataType
{

    private long[] Data { get; } = new long[(bitCapacity + 63) / 64];

    public bool this[int index]
    {
        get => (Data[index / 64] & (1L << (index % 64))) != 0;
        set
        {
            if (value) Data[index / 64] |= 1L << (index % 64);
            else Data[index / 64] &= ~(1L << (index % 64));
        }
    }

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WritePrefixedArray(Data, static (writer, @long) => writer.WriteInt64(@long));
    }

}
