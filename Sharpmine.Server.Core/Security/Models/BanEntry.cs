namespace Sharpmine.Server.Core.Security.Models;

public record BanEntry(
    Guid Uuid,
    string Name,
    DateTimeOffset Created,
    string Source,
    DateTimeOffset Expires,
    string Reason);
