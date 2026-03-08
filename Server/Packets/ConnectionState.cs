namespace Sharpmine.Server.Packets;

public enum ConnectionState : byte
{

    Handshake,

    Status,

    Login,

    Configuration,

    Play,

}