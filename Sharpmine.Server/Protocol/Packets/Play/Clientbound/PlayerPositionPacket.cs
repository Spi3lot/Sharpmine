namespace Sharpmine.Server.Protocol.Packets.Play.Clientbound;

public partial record PlayerPositionPacket(
    int TeleportId,
    double X,
    double Y,
    double Z,
    double VelocityX,
    double VelocityY,
    double VelocityZ,
    float Yaw,
    float Pitch,
    TeleportRelativeAxes Flags)
{

    public Task SerializeContentAsync(
        Stream stream,
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        writer.Write7BitEncodedInt(TeleportId);
        writer.Write(X);
        writer.Write(Y);
        writer.Write(Z);
        writer.Write(VelocityX);
        writer.Write(VelocityY);
        writer.Write(VelocityZ);
        writer.Write(Yaw);
        writer.Write(Pitch);
        writer.Write((int) Flags);
        return Task.CompletedTask;
    }

}