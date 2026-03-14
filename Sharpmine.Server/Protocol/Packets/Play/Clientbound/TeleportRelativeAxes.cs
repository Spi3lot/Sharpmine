namespace Sharpmine.Server.Protocol.Packets.Play.Clientbound;

[Flags]
public enum TeleportRelativeAxes
{

    X = 0x0001,
    Y = 0x0002,
    Z = 0x0004,

    Yaw = 0x0008,
    Pitch = 0x0010,

    VelocityX = 0x0020,
    VelocityY = 0x0040,
    VelocityZ = 0x0080,

    RotateBeforeVelocityChange = 0x0100,

}