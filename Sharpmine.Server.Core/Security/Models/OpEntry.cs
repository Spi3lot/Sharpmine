namespace Sharpmine.Server.Core.Security.Models;

public record OpEntry(
    Guid Uuid,
    string Name,
    int Level,
    bool BypassesPlayerLimit);
