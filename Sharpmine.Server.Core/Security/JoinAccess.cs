namespace Sharpmine.Server.Core.Security;

public enum JoinAccess : byte
{

    Allowed,

    ServerFull,

    NotWhitelisted,

    Banned,

    IpBanned,

    IpBlacklisted,

}
