namespace Sharpmine.Server.Protocol.Packets;

public interface IServerboundPacket : IPacket
{

    Task DeserializeContentAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken) => throw new NotImplementedException();

    Task ProcessAsync(
        ClientHandler handler,
        NetworkStream stream,
        BinaryReader reader,
        BinaryWriter writer,
        CancellationToken cancellationToken) => throw new NotImplementedException();

    string ToString();

}
