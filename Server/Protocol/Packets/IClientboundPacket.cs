using System.Net.Sockets;

namespace Sharpmine.Server.Protocol.Packets;

public interface IClientboundPacket
{
    
    int Id { get; }

    Task SerializeAsync(NetworkStream stream, BinaryWriter writer);

}