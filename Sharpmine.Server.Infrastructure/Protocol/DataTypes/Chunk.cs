using System.Buffers;

using Sharpmine.Domain.Registries.Dynamic.Entries;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public sealed class Chunk : IClientboundDataType
{

    [ThreadStatic]
    private static ArrayBufferWriter<byte>? _chunkDataWriter;

    private readonly int _minY;

    public Chunk(DimensionType dimension)
    {
        _minY = dimension.MinY;
        Sections = new ChunkSection[dimension.Height / 16];

        for (int i = 0; i < Sections.Length; i++)
        {
            Sections[i] = new ChunkSection();
        }
    }

    public ChunkSection[] Sections { get; }

    public ref readonly ChunkSection GetSection(int blockY) => ref Sections[(blockY - _minY) / 16];

    public void Serialize(IBufferWriter<byte> writer)
    {
        _chunkDataWriter ??= new ArrayBufferWriter<byte>(131_072);
        _chunkDataWriter.WriteArray(Sections);
    }

}
