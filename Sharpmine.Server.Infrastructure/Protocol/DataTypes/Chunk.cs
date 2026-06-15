using Sharpmine.Domain.Registries.Dynamic.Entries;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public sealed class Chunk
{

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

    public ChunkSection GetSection(int blockY) => Sections[(blockY - _minY) / 16];

}
