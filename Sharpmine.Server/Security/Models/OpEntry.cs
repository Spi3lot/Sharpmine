using Sharpmine.Server.Protocol.DataTypes;

namespace Sharpmine.Server.Security.Models;

public record OpEntry(
    Uuid Uuid,
    string Name,
    int Level,
    bool BypassesPlayerLimit);
