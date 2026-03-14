using System.Net.Sockets;

namespace Sharpmine.Server.Protocol.Packets;

public interface IClientboundPacket : IPacket
{

    Task SerializeContentAsync(NetworkStream stream, BinaryWriter writer) => throw new NotImplementedException();

}