namespace Sharpmine.Server.Protocol.Packets;

public interface IPacket
{

    ProtocolState State { get; }

    int Id { get; }

}