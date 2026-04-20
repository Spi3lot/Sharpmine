namespace Sharpmine.Server.Protocol.Packets;

public interface IServerboundPacket : IPacket
{

    // TODO: make synchronous when using System.IO.Pipelines
    Task DeserializeContentAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken) => throw new NotImplementedException();

    ValueTask ProcessAsync(ClientHandler handler, CancellationToken cancellationToken) => throw new NotImplementedException();

    string ToString();

}
