namespace Sharpmine.Server.Protocol;

public enum ConnectionState : byte
{

    Handshake,

    Status,

    Login,

    Configuration,

    Play,

}