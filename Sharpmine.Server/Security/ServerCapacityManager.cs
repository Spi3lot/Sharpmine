using Microsoft.Extensions.Logging;

using Sharpmine.Server.Configuration;

namespace Sharpmine.Server.Security;

public partial class ServerCapacityManager(ServerProperties properties, ILogger<ServerCapacityManager> logger)
{

    private readonly Lock _capacityLock = new();

    private int _onlinePlayerCount;

    public int OnlinePlayerCount
    {
        get
        {
            lock (_capacityLock)
            {
                return _onlinePlayerCount;
            }
        }
    }

    public bool TryReserveSlot(bool bypassLimit)
    {
        lock (_capacityLock)
        {
            if (_onlinePlayerCount >= properties.MaxPlayers && !bypassLimit)
            {
                return false;
            }

            _onlinePlayerCount++;
            return true;
        }
    }

    public void ReleaseSlot()
    {
        lock (_capacityLock)
        {
            if (_onlinePlayerCount <= 0)
            {
                LogAttemptedPlayerSlotReleaseAlthoughNoneReserved();
                return;
            }

            _onlinePlayerCount--;
        }
    }

}
