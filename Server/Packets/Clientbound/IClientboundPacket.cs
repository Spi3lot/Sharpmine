using System.Net.Sockets;

namespace Sharpmine.Server.Packets.Clientbound;

public interface IClientboundPacket
{
    
    int Id { get; }

    Task SerializeAsync(NetworkStream stream, BinaryWriter writer);

}