using Sharpmine.Server.Protocol.DataTypes;

namespace Sharpmine.Server.Security.Models;

public record WhitelistEntry(Uuid Uuid, string Name);
