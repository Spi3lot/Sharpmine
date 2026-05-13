using System.Buffers;

using Sharpmine.Server.Core.Protocol.Extensions;

namespace Sharpmine.Server.Core.Protocol.DataTypes;

public readonly record struct Position(int X, short Y, int Z) : IBidirectionalDataType<Position>
{

    public static bool TryDeserialize(ref SequenceReader<byte> reader, out Position value)
    {
        value = default;
        if (!reader.TryReadBigEndian(out long val)) return false;

        value = new Position(
            (int) (val >> 38),
            (short) ((val << 52) >> 52),
            (int) ((val << 26) >> 26));

        return true;
    }

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteInt64(((X & 0x3FFFFFFL) << 38) | ((Z & 0x3FFFFFFL) << 12) | (Y & 0xFFFL));
    }

}
