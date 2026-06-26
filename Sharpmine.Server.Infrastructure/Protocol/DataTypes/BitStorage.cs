namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public readonly struct BitStorage
{

    private readonly int _bitsPerEntry;

    private readonly int _valuesPerLong;

    private readonly long _mask;

    public BitStorage(int bitsPerEntry, int capacity)
    {
        _bitsPerEntry = bitsPerEntry;
        _valuesPerLong = 64 / bitsPerEntry;
        _mask = (1L << bitsPerEntry) - 1;
        Data = new long[(capacity + _valuesPerLong - 1) / _valuesPerLong];
    }

    public long[] Data { get; }

    public int this[int index]
    {
        get
        {
            var divRem = int.DivRem(index, _valuesPerLong);
            int longIndex = divRem.Quotient;
            int bitOffset = divRem.Remainder * _bitsPerEntry;
            return (int) ((Data[longIndex] >> bitOffset) & _mask);
        }
        set
        {
            var divRem = int.DivRem(index, _valuesPerLong);
            int longIndex = divRem.Quotient;
            int bitOffset = divRem.Remainder * _bitsPerEntry;
            Data[longIndex] = (Data[longIndex] & ~(_mask << bitOffset)) | ((value & _mask) << bitOffset);
        }
    }

}
