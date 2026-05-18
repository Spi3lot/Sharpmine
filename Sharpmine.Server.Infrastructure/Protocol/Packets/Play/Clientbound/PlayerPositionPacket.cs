using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Clientbound;

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

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteVarInt(TeleportId);
        writer.WriteDouble(X);
        writer.WriteDouble(Y);
        writer.WriteDouble(Z);
        writer.WriteDouble(VelocityX);
        writer.WriteDouble(VelocityY);
        writer.WriteDouble(VelocityZ);
        writer.WriteSingle(Yaw);
        writer.WriteSingle(Pitch);
        writer.WriteInt32((int) Flags);
    }

}
