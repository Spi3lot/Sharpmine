using Sharpmine.Server.Protocol.DataTypes;

namespace Sharpmine.Server.Security.Models;

public record BanEntry(
    Uuid Uuid,
    string Name,
    DateTimeOffset Created,
    string Source,
    DateTimeOffset Expires,
    string Reason);
