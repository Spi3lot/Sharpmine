using System.Buffers;

using Raspite.Tags;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public sealed record BlockEntity(byte PackedXz, short Y, int TypeId, CompoundTag Data)
{

    public BlockEntity(int blockX, short y, int blockZ, int typeId, CompoundTag data)
        : this((byte) ((blockX & 15) << 4 | (blockZ & 15)), y, typeId, data)
    {
    }

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteByte(PackedXz);
        writer.WriteInt16(Y);
        writer.WriteVarInt(TypeId);
        writer.WriteNbt(Data, network: true);
    }

}
