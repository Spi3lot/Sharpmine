namespace Sharpmine.Server.Protocol.Packets.Login.Serverbound;

public partial record LoginAcknowledgedPacket
{

    public Task DeserializeContentAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken) => Task.CompletedTask;

    public ValueTask ProcessAsync(
        ClientHandler handler,
        CancellationToken cancellationToken)
    {
        handler.TransitionTo(ProtocolState.Configuration);
        return ValueTask.CompletedTask;
    }

}
