using System.Text.Json;

using Microsoft.Extensions.Logging;

using Sharpmine.Server.Configuration;
using Sharpmine.Server.Security.Models;

namespace Sharpmine.Server.Security;

public partial class PlayerAccessManager
{

    private readonly ServerProperties _properties;

    private readonly ILogger<PlayerAccessManager> _logger;

    private readonly HashSet<string> _blacklistedIps;

    private readonly Dictionary<string, IpBanEntry> _bannedIps;

    private readonly Dictionary<Guid, BanEntry> _bannedPlayers;

    private readonly Dictionary<Guid, WhitelistEntry> _whitelistedPlayers;

    private readonly Dictionary<Guid, OpEntry> _ops;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };

    public PlayerAccessManager(ServerProperties properties, ILogger<PlayerAccessManager> logger)
    {
        _properties = properties;
        _logger = logger;

        _blacklistedIps = LoadJson<string>(ServerConstants.BlacklistedIpsFileName)
            .ToHashSet();

        _bannedIps = LoadJson<IpBanEntry>(ServerConstants.BannedIpsFileName)
            .ToDictionary(e => e.Ip);

        _bannedPlayers = LoadJson<BanEntry>(ServerConstants.BannedPlayersFileName)
            .ToDictionary(e => e.Uuid);

        _whitelistedPlayers = LoadJson<WhitelistEntry>(ServerConstants.WhitelistedPlayersFileName)
            .ToDictionary(e => e.Uuid);

        _ops = LoadJson<OpEntry>(ServerConstants.OperatorsFileName)
            .ToDictionary(e => e.Uuid);

        SaveAll();
    }

    public (JoinAccess Access, string? Reason) EvaluateAccess(string ip, Guid? uuid = null)
    {
        if (_blacklistedIps.Contains(ip))
        {
            return (JoinAccess.IpBlacklisted, null);
        }

        if (_bannedIps.TryGetValue(ip, out var ipBan))
        {
            return (JoinAccess.IpBanned, $"You are banned from this server.\nReason: {ipBan.Reason}");
        }

        if (!uuid.HasValue)
        {
            return (JoinAccess.Allowed, null);
        }

        if (_bannedPlayers.TryGetValue(uuid.Value, out var ban))
        {
            return (JoinAccess.Banned, $"You are banned from this server.\nReason: {ban.Reason}");
        }

        if (_properties.WhiteList && !IsImplicitlyWhitelisted(uuid.Value))
        {
            return (JoinAccess.NotWhitelisted, "You are not white-listed on this server!");
        }

        return (JoinAccess.Allowed, null);
    }

    public bool IsImplicitlyWhitelisted(Guid playerId) => IsExplicitlyWhitelisted(playerId) || IsOp(playerId);

    public bool IsExplicitlyWhitelisted(Guid playerId) => _whitelistedPlayers.ContainsKey(playerId);

    public bool IsOp(Guid playerId) => _ops.ContainsKey(playerId);

    public int GetOpLevel(Guid playerId) => _ops.TryGetValue(playerId, out var op) ? op.Level : 0;

    public bool BypassesPlayerLimit(Guid playerId) => _ops.TryGetValue(playerId, out var op) && op.BypassesPlayerLimit;

    public void SaveAll()
    {
        SaveToFile(ServerConstants.BlacklistedIpsFileName, _blacklistedIps);
        SaveToFile(ServerConstants.BannedIpsFileName, _bannedIps.Values);
        SaveToFile(ServerConstants.BannedPlayersFileName, _bannedPlayers.Values);
        SaveToFile(ServerConstants.WhitelistedPlayersFileName, _whitelistedPlayers.Values);
        SaveToFile(ServerConstants.OperatorsFileName, _ops.Values);
    }

    private static void SaveToFile<T>(string fileName, IEnumerable<T> data)
    {
        string json = JsonSerializer.Serialize(data, JsonOptions);
        File.WriteAllText(fileName, json);
    }

    private List<T> LoadJson<T>(string fileName)
    {
        try
        {
            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName, "[]");
                return [];
            }

            string json = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<List<T>>(json, JsonOptions) ?? [];
        }
        catch (Exception ex)
        {
            LogErrorWhileLoadingJson(ex);
            return [];
        }
    }

}
