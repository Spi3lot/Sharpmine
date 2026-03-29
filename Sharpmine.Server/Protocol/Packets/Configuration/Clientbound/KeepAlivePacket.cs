namespace Sharpmine.Server.Protocol.Packets.Configuration.Clientbound;

public partial record KeepAlivePacket(long KeepAliveId)
{

    public Task SerializeContentAsync(
        Stream stream,
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        writer.Write(KeepAliveId);
        return Task.CompletedTask;
    }

}
