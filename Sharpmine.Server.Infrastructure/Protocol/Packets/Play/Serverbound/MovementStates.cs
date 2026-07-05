namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Serverbound;

[Flags]
public enum MovementStates : sbyte
{

    None = 0x00,

    OnGround = 0x01,

    PushingAgainstWall = 0x02,

}
