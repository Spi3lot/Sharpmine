namespace Sharpmine.Server.Protocol.Packets;

public interface IServerboundPacket : IPacket
{

    // TODO: Consider ValueTask
    static abstract Task DeserializeAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken);

    // TODO: Consider ValueTask
    Task ProcessAsync(
        ClientHandler handler,
        NetworkStream stream,
        BinaryReader reader,
        BinaryWriter writer,
        CancellationToken cancellationToken) => throw new NotImplementedException();

    string ToString();

}
