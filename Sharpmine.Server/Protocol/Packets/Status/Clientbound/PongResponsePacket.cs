namespace Sharpmine.Server.Protocol.Packets.Status.Clientbound;

public partial record PongResponsePacket(long Timestamp)
{

    public Task SerializeContentAsync(
        Stream stream,
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        writer.Write7BitEncodedInt64(Timestamp);
        return Task.CompletedTask;
    }

}