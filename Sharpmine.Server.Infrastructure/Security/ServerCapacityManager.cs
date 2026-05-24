using System.Collections.Immutable;

using Sharpmine.Server.Infrastructure.Configuration;
using Sharpmine.Server.Infrastructure.Protocol;
using Sharpmine.Server.Infrastructure.Protocol.DataTypes;

namespace Sharpmine.Server.Infrastructure.Security;

public class ServerCapacityManager(ServerProperties properties)
{

    private readonly Lock _capacityLock = new();

    private readonly Dictionary<Guid, StatusPlayer> _clientIdToPlayer = [];

    private readonly Dictionary<Guid, ClientHandler> _playerIdToClient = [];

    public int OnlinePlayerCount
    {
        get
        {
            lock (_capacityLock)
            {
                return _clientIdToPlayer.Count;
            }
        }
    }

    public ImmutableArray<StatusPlayer> GetOnlinePlayers()
    {
        lock (_capacityLock)
        {
            return [.. _clientIdToPlayer.Values];
        }
    }

    public ImmutableArray<StatusPlayer> GetListablePlayers()
    {
        lock (_capacityLock)
        {
            var listablePlayers = _clientIdToPlayer.Values
                .Where(player => _playerIdToClient[player.Id].Information is { AllowServerListings: true });

            return [.. listablePlayers];
        }
    }

    public bool TryReserveSlot(ClientHandler client, StatusPlayer player, bool bypassLimit)
    {
        lock (_capacityLock)
        {
            if (_clientIdToPlayer.Count >= properties.MaxPlayers && !bypassLimit)
            {
                return false;
            }

            _clientIdToPlayer[client.Id] = player;
            _playerIdToClient[player.Id] = client;
            return true;
        }
    }

    public bool TryReleaseSlot(Guid clientId)
    {
        lock (_capacityLock)
        {
            return _clientIdToPlayer.Remove(clientId, out var player)
                   && _playerIdToClient.Remove(player.Id);
        }
    }

}
