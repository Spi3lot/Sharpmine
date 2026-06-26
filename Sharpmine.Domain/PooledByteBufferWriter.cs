namespace Sharpmine.Domain;

using System.Buffers;

public sealed class PooledByteBufferWriter(
    int initialCapacity = PooledByteBufferWriter.DefaultInitialBufferSize,
    bool clearBufferOnDispose = false) : IBufferWriter<byte>, IDisposable
{

    public const int DefaultInitialBufferSize = 256;

    private const int ArrayMaxLength = 0x7FFFFFC7;

    private byte[]? _buffer = ArrayPool<byte>.Shared.Rent(initialCapacity);

    public ReadOnlyMemory<byte> WrittenMemory => _buffer.AsMemory(0, WrittenCount);

    public ReadOnlySpan<byte> WrittenSpan => _buffer.AsSpan(0, WrittenCount);

    public int WrittenCount { get; private set; }

    public int Capacity => _buffer!.Length;

    public int FreeCapacity => _buffer!.Length - WrittenCount;

    public void Clear()
    {
        _buffer.AsSpan(0, WrittenCount).Clear();
        WrittenCount = 0;
    }

    public void ResetWrittenCount() => WrittenCount = 0;

    public void Advance(int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(count, FreeCapacity);
        WrittenCount += count;
    }

    public Memory<byte> GetMemory(int sizeHint = 0)
    {
        CheckAndResizeBuffer(sizeHint);
        return _buffer.AsMemory(WrittenCount);
    }

    public Span<byte> GetSpan(int sizeHint = 0)
    {
        CheckAndResizeBuffer(sizeHint);
        return _buffer.AsSpan(WrittenCount);
    }

    private void CheckAndResizeBuffer(int sizeHint)
    {
        if (sizeHint == 0)
        {
            sizeHint = 1;
        }

        if (sizeHint > FreeCapacity)
        {
            int growBy = Math.Max(sizeHint, Capacity);

            if (Capacity == 0)
            {
                growBy = Math.Max(growBy, DefaultInitialBufferSize);
            }

            int newSize = unchecked(Capacity + growBy);

            if ((uint) newSize > ArrayMaxLength)
            {
                uint needed = (uint) unchecked(WrittenCount + sizeHint);

                if (needed > ArrayMaxLength)
                {
                    throw new OutOfMemoryException($"Buffer cannot exceed max array length ({needed} > {ArrayMaxLength})");
                }

                newSize = ArrayMaxLength;
            }

            byte[] newBuffer = ArrayPool<byte>.Shared.Rent(newSize);
            Array.Copy(_buffer!, newBuffer, WrittenCount);
            ArrayPool<byte>.Shared.Return(_buffer!, clearBufferOnDispose);
            _buffer = newBuffer;
        }
    }

    public void Dispose()
    {
        if (_buffer is not null)
        {
            ArrayPool<byte>.Shared.Return(_buffer, clearBufferOnDispose);
            _buffer = null;
        }
    }

}
