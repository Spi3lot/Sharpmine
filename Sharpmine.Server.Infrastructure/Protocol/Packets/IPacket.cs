namespace Sharpmine.Server.Infrastructure.Protocol.Packets;

public interface IPacket
{

    ProtocolState State { get; }

    int Id { get; }

}
