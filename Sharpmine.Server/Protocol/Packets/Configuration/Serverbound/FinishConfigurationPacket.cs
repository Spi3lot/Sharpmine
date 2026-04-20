namespace Sharpmine.Server.Protocol.Packets.Configuration.Serverbound;

public partial record FinishConfigurationPacket
{

    public Task DeserializeContentAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken) => Task.CompletedTask;

    public ValueTask ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        handler.SwitchProtocolState(ProtocolState.Play);
        return ValueTask.CompletedTask;
    }

}
