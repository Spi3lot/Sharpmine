namespace Sharpmine.Server.Protocol.Packets.Configuration.Serverbound;

public partial record FinishConfigurationPacket
{

    public static Task DeserializeAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken) => Task.CompletedTask;

    public Task ProcessAsync(
        ClientHandler handler,
        NetworkStream stream,
        BinaryReader reader,
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        handler.SwitchProtocolState(ProtocolState.Play);
        return Task.CompletedTask;
    }

}
