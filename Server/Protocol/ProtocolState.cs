namespace Sharpmine.Server.Protocol;

public enum ProtocolState : byte
{

    Handshake = 0,

    Status = 1,

    Login = 2,

    Configuration = 3,

    Play = 4,

}