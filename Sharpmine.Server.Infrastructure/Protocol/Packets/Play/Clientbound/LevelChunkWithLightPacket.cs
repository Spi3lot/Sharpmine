using System.Buffers;

using Raspite.Tags;

using Sharpmine.Server.Infrastructure.Protocol.DataTypes;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Clientbound;

public partial record LevelChunkWithLightPacket(
    int ChunkX,
    int ChunkZ,
    CompoundTag Heightmaps,
    Chunk Column,
    BlockEntity[] BlockEntities,
    LightData LightData)
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteInt32(ChunkX);
        writer.WriteInt32(ChunkZ);
        writer.WriteNbt(Heightmaps, network: true);
        var chunkDataWriter = new ArrayBufferWriter<byte>(131_072);
        chunkDataWriter.WriteArray(Column.Sections);
        writer.WritePrefixed(chunkDataWriter.WrittenSpan);
        writer.WritePrefixedArray(BlockEntities, static (writer, entity) => entity.Serialize(writer));
        writer.Write(LightData);
    }

}
