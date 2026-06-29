using System.Buffers;

using Sharpmine.Domain.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Clientbound;

public partial record SetChunkCacheCenterPacket(int ChunkX, int ChunkZ)
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteVarInt(ChunkX);
        writer.WriteVarInt(ChunkZ);
    }

}
