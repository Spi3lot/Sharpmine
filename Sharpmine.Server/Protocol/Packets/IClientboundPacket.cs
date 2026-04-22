namespace Sharpmine.Server.Protocol.Packets;

public interface IClientboundPacket : IPacket
{

    void SerializeContent(Stream stream, BinaryWriter writer)
        => throw new NotImplementedException();

    string ToString();

}
