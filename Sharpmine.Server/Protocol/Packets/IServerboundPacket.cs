namespace Sharpmine.Server.Protocol.Packets;

public interface IServerboundPacket : IPacket
{

    bool DeserializeContent(NetworkStream stream, BinaryReader reader)
        => throw new NotImplementedException();

    ValueTask ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    string ToString();

}
