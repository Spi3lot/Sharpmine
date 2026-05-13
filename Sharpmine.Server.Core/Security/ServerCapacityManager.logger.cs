using Microsoft.Extensions.Logging;

namespace Sharpmine.Server.Core.Security;

public partial class ServerCapacityManager
{

    [LoggerMessage(LogLevel.Warning, "Attempted to release a player slot when none were reserved")]
    partial void LogAttemptedPlayerSlotReleaseAlthoughNoneReserved();

}
