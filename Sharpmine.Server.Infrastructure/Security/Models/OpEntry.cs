namespace Sharpmine.Server.Infrastructure.Security.Models;

public record OpEntry(
    Guid Uuid,
    string Name,
    int Level,
    bool BypassesPlayerLimit);
