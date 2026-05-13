namespace Sharpmine.Server.Core.Security.Models;

public record IpBanEntry(
    string Ip,
    DateTimeOffset Created,
    string Source,
    DateTimeOffset Expires,
    string Reason);
