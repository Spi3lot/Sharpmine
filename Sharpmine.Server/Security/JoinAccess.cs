namespace Sharpmine.Server.Security;

public enum JoinAccess : byte
{

    Allowed,

    ServerFull,

    NotWhitelisted,

    Banned,

    IpBanned,

    IpBlacklisted,

}
