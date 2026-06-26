using System.Buffers;

using Raspite.Tags;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public readonly record struct BlockEntity(byte PackedXz, short Y, int TypeId, CompoundTag Data)
{

    public BlockEntity(int x, int y, int z, int typeId, CompoundTag data)
        : this((byte) (((x & 15) << 4) | (z & 15)), (short) y, typeId, data)
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
