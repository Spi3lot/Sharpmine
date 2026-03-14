using System.Net.Sockets;

namespace Sharpmine.Server.Protocol.Packets.Play.Clientbound;

public partial record TeleportEntityPacket(
    int EntityId,
    double X,
    double Y,
    double Z,
    double VelocityX,
    double VelocityY,
    double VelocityZ,
    float Yaw,
    float Pitch,
    TeleportRelativeAxes Flags,
    bool OnGround
)
{

    public Task SerializeContentAsync(NetworkStream stream, BinaryWriter writer)
    {
        writer.Write7BitEncodedInt(EntityId);
        writer.Write(X);
        writer.Write(Y);
        writer.Write(Z);
        writer.Write(VelocityX);
        writer.Write(VelocityY);
        writer.Write(VelocityZ);
        writer.Write(Yaw);
        writer.Write(Pitch);
        writer.Write((int) Flags);
        writer.Write(OnGround);
        return Task.CompletedTask;
    }

}