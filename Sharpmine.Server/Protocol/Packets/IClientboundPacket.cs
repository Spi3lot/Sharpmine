namespace Sharpmine.Server.Protocol.Packets;

public interface IClientboundPacket : IPacket
{

    Task SerializeContentAsync(
        Stream stream,
        BinaryWriter writer,
        CancellationToken cancellationToken) => throw new NotImplementedException();

}
