namespace Sharpmine.Server.Protocol.Packets;

public interface IServerboundPacket : IPacket
{

    ProtocolResult DeserializeContent(NetworkStream stream, BinaryReader reader)
        => ProtocolResult.NotImplemented;

    ValueTask<ProtocolResult> ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
        => ValueTask.FromResult(ProtocolResult.NotImplemented);

    string ToString();

}
