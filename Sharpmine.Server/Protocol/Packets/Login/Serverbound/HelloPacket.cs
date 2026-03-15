using System.Net.Sockets;

namespace Sharpmine.Server.Protocol.Packets.Login.Serverbound;

public partial record HelloPacket
{

    public Task DeserializeContentAsync(NetworkStream stream, BinaryReader reader)
    {
        throw new NotImplementedException();
    }

    public Task ProcessAsync(ClientHandler handler, NetworkStream stream, BinaryReader reader, BinaryWriter writer)
    {
        throw new NotImplementedException();
    }

}