namespace Sharpmine.Server.Protocol.Packets.Configuration.Clientbound;

public partial record FinishConfigurationPacket
{

    public Task SerializeContentAsync(
        Stream stream,
        BinaryWriter writer,
        CancellationToken cancellationToken) => Task.CompletedTask;

}
