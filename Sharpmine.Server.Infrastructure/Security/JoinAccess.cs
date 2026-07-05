namespace Sharpmine.Server.Infrastructure.Security;

public enum JoinAccess : byte
{

    Allowed,

    NotWhitelisted,

    Banned,

    IpBanned,

    IpBlacklisted,

}
