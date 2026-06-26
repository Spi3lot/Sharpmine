using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.DataTypes;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Clientbound;

public partial record LevelChunkWithLightPacket(
    int ChunkX,
    int ChunkZ,
    Heightmap[] Heightmaps,
    Chunk Column,
    BlockEntity[] BlockEntities,
    LightData LightData)
{

    public LevelChunkWithLightPacket(int chunkX, int chunkZ, Chunk column, LightData lightData)
        : this(chunkX, chunkZ, column.Heightmaps, column, column.BlockEntities, lightData)
    {
    }

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteInt32(ChunkX);
        writer.WriteInt32(ChunkZ);
        writer.WritePrefixedArray(Heightmaps);
        writer.Write(Column);
        writer.WritePrefixedArray(BlockEntities, static (writer, entity) => entity.Serialize(writer));
        writer.Write(LightData);
    }

}
