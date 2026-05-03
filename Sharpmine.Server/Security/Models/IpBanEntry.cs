namespace Sharpmine.Server.Security.Models;

public record IpBanEntry(
    string Ip,
    DateTimeOffset Created,
    string Source,
    DateTimeOffset Expires,
    string Reason);
