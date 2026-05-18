namespace Sharpmine.Server.Infrastructure.Security;

public enum JoinAccess : byte
{

    Allowed,

    ServerFull,

    NotWhitelisted,

    Banned,

    IpBanned,

    IpBlacklisted,

}
