namespace Sharpmine.Server;

public enum ConnectionState : byte
{

    Handshake,

    Status,

    Login,

    Configuration,

    Play,

}