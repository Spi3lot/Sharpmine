namespace Sharpmine.Server.Protocol.Packets;

public interface IClientboundPacket : IPacket
{

    // TODO: Consider ValueTask
    Task SerializeContentAsync(
        Stream stream,
        BinaryWriter writer,
        CancellationToken cancellationToken) => throw new NotImplementedException();

    string ToString();

}
