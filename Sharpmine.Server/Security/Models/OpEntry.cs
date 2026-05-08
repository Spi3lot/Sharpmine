namespace Sharpmine.Server.Security.Models;

public record OpEntry(
    Guid Uuid,
    string Name,
    int Level,
    bool BypassesPlayerLimit);
