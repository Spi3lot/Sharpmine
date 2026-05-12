using System.Buffers;

using Sharpmine.Server.Protocol.Extensions;

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
    bool OnGround)
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteVarInt(EntityId);
        writer.WriteDouble(X);
        writer.WriteDouble(Y);
        writer.WriteDouble(Z);
        writer.WriteDouble(VelocityX);
        writer.WriteDouble(VelocityY);
        writer.WriteDouble(VelocityZ);
        writer.WriteSingle(Yaw);
        writer.WriteSingle(Pitch);
        writer.WriteInt32((int) Flags);
        writer.WriteBoolean(OnGround);
    }

}
