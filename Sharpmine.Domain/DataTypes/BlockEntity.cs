using System.Buffers;

using Raspite.Tags;

using Sharpmine.Domain.Extensions;

namespace Sharpmine.Domain.DataTypes;

public readonly record struct BlockEntity(byte PackedXz, short Y, int TypeId, CompoundTag Data)
{

    public BlockEntity(int x, int y, int z, int typeId, CompoundTag data)
        : this((byte) ((x << 4) | z), (short) y, typeId, data)
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
