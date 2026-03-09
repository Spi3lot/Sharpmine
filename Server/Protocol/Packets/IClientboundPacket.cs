using System.Net.Sockets;

namespace Sharpmine.Server.Protocol.Packets;

public interface IClientboundPacket
{

    ProtocolState State { get; }

    int Id { get; }

    Task SerializeAsync(NetworkStream stream, BinaryWriter writer) => throw new NotImplementedException();

}