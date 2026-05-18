using System.Collections.Immutable;

using Sharpmine.Server.Infrastructure.Configuration;
using Sharpmine.Server.Infrastructure.Protocol.DataTypes;

namespace Sharpmine.Server.Infrastructure.Security;

public class ServerCapacityManager(ServerProperties properties)
{

    private readonly Lock _capacityLock = new();

    private readonly Dictionary<Guid, StatusPlayer> _onlinePlayers = [];

    public int OnlinePlayerCount
    {
        get
        {
            lock (_capacityLock)
            {
                return _onlinePlayers.Count;
            }
        }
    }

    public ImmutableArray<StatusPlayer> GetOnlinePlayers()
    {
        lock (_capacityLock)
        {
            return [.. _onlinePlayers.Values];
        }
    }

    public bool TryReserveSlot(Guid id, StatusPlayer player, bool bypassLimit)
    {
        lock (_capacityLock)
        {
            if (_onlinePlayers.Count >= properties.MaxPlayers && !bypassLimit)
            {
                return false;
            }

            _onlinePlayers[id] = player;
            return true;
        }
    }

    public bool TryReleaseSlot(Guid id)
    {
        lock (_capacityLock)
        {
            return _onlinePlayers.Remove(id);
        }
    }

}
