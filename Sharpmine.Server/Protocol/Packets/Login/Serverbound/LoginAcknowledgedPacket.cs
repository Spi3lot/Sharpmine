namespace Sharpmine.Server.Protocol.Packets.Login.Serverbound;

public partial record LoginAcknowledgedPacket : IStateTransition
{

    public ProtocolState NextState => ProtocolState.Configuration;

    public Task DeserializeContentAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken) => Task.CompletedTask;

    public ValueTask ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

}
