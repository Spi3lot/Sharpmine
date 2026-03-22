namespace Sharpmine.Server.Protocol.DataTypes;

public readonly record struct Position(int X, short Y, int Z) : IBidirectionalDataType<Position>
{

    public static Position Deserialize(BinaryReader reader)
    {
        long val = reader.ReadInt64();

        return new Position(
            (int) (val >> 38),
            (short) ((val << 52) >> 52),
            (int) ((val << 26) >> 26));
    }

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(((X & 0x3FFFFFFL) << 38) | ((Z & 0x3FFFFFFL) << 12) | (Y & 0xFFFL));
    }

}
