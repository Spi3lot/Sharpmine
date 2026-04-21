namespace Sharpmine.Server.Protocol.Packets;

public interface IClientboundPacket : IPacket
{

    // TODO: make synchronous when using System.IO.Pipelines
    Task SerializeContentAsync(
        Stream stream,
        BinaryWriter writer,
        CancellationToken cancellationToken) => throw new NotImplementedException();

    string ToString();

}
