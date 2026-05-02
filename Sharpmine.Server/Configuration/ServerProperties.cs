using Microsoft.Extensions.Configuration;

namespace Sharpmine.Server.Configuration;

public class ServerProperties
{

    #region NETWORK & CONNECTION

    [ConfigurationKeyName("server-ip")]
    public string ServerIp { get; set; } = string.Empty;

    [ConfigurationKeyName("server-port")]
    public ushort ServerPort { get; set; } = 25565;

    [ConfigurationKeyName("online-mode")]
    public bool OnlineMode { get; set; } = true;

    [ConfigurationKeyName("network-compression-threshold")]
    public int NetworkCompressionThreshold { get; set; } = 256;

    [ConfigurationKeyName("prevent-proxy-connections")]
    public bool PreventProxyConnections { get; set; } = false;

    [ConfigurationKeyName("use-native-transport")]
    public bool UseNativeTransport { get; set; } = true;

    [ConfigurationKeyName("accepts-transfers")]
    public bool AcceptsTransfers { get; set; } = false;

    #endregion

    #region GAMEPLAY & SERVER IDENTITY

    [ConfigurationKeyName("motd")]
    public string Motd { get; set; } = "A Minecraft Server";

    [ConfigurationKeyName("max-players")]
    public int MaxPlayers { get; set; } = 20;

    [ConfigurationKeyName("gamemode")]
    public string Gamemode { get; set; } = "survival";

    [ConfigurationKeyName("force-gamemode")]
    public bool ForceGamemode { get; set; } = false;

    [ConfigurationKeyName("difficulty")]
    public string Difficulty { get; set; } = "easy";

    [ConfigurationKeyName("hardcore")]
    public bool Hardcore { get; set; } = false;

    [ConfigurationKeyName("pvp")]
    public bool Pvp { get; set; } = true;

    [ConfigurationKeyName("view-distance")]
    public int ViewDistance { get; set; } = 10;

    [ConfigurationKeyName("simulation-distance")]
    public int SimulationDistance { get; set; } = 10;

    [ConfigurationKeyName("player-idle-timeout")]
    public int PlayerIdleTimeout { get; set; } = 0;

    [ConfigurationKeyName("hide-online-players")]
    public bool HideOnlinePlayers { get; set; } = false;

    #endregion

    #region WORLD GENERATION & MECHANICS

    [ConfigurationKeyName("level-name")]
    public string LevelName { get; set; } = "world";

    [ConfigurationKeyName("level-seed")]
    public string LevelSeed { get; set; } = string.Empty;

    [ConfigurationKeyName("level-type")]
    public string LevelType { get; set; } = "minecraft:normal";

    [ConfigurationKeyName("allow-nether")]
    public bool AllowNether { get; set; } = true;

    [ConfigurationKeyName("allow-flight")]
    public bool AllowFlight { get; set; } = false;

    [ConfigurationKeyName("generate-structures")]
    public bool GenerateStructures { get; set; } = true;

    [ConfigurationKeyName("max-world-size")]
    public int MaxWorldSize { get; set; } = 29999984;

    [ConfigurationKeyName("max-chained-neighbor-updates")]
    public int MaxChainedNeighborUpdates { get; set; } = 1000000;

    [ConfigurationKeyName("sync-chunk-writes")]
    public bool SyncChunkWrites { get; set; } = true;

    #endregion

    #region ENTITIES & SPAWNING

    [ConfigurationKeyName("spawn-animals")]
    public bool SpawnAnimals { get; set; } = true;

    [ConfigurationKeyName("spawn-monsters")]
    public bool SpawnMonsters { get; set; } = true;

    [ConfigurationKeyName("spawn-npcs")]
    public bool SpawnNpcs { get; set; } = true;

    [ConfigurationKeyName("spawn-protection")]
    public int SpawnProtection { get; set; } = 16;

    [ConfigurationKeyName("entity-broadcast-range-percentage")]
    public int EntityBroadcastRangePercentage { get; set; } = 100;

    #endregion

    #region SECURITY, COMMANDS & OPS

    [ConfigurationKeyName("white-list")]
    public bool WhiteList { get; set; } = false;

    [ConfigurationKeyName("enforce-secure-profile")]
    public bool EnforceSecureProfile { get; set; } = true;

    [ConfigurationKeyName("enable-command-block")]
    public bool EnableCommandBlock { get; set; } = false;

    [ConfigurationKeyName("op-permission-level")]
    public int OpPermissionLevel { get; set; } = 4;

    [ConfigurationKeyName("function-permission-level")]
    public int FunctionPermissionLevel { get; set; } = 2;

    [ConfigurationKeyName("broadcast-console-to-ops")]
    public bool BroadcastConsoleToOps { get; set; } = true;

    [ConfigurationKeyName("broadcast-rcon-to-ops")]
    public bool BroadcastRconToOps { get; set; } = true;

    [ConfigurationKeyName("rate-limit")]
    public int RateLimit { get; set; } = 0;

    #endregion

    #region RESOURCE PACK

    [ConfigurationKeyName("require-resource-pack")]
    public bool RequireResourcePack { get; set; } = false;

    [ConfigurationKeyName("resource-pack")]
    public string ResourcePack { get; set; } = string.Empty;

    [ConfigurationKeyName("resource-pack-prompt")]
    public string ResourcePackPrompt { get; set; } = string.Empty;

    [ConfigurationKeyName("resource-pack-sha1")]
    public string ResourcePackSha1 { get; set; } = string.Empty;

    [ConfigurationKeyName("resource-pack-id")]
    public string ResourcePackId { get; set; } = string.Empty;

    #endregion

    #region STATUS, QUERY & RCON

    [ConfigurationKeyName("enable-status")]
    public bool EnableStatus { get; set; } = true;

    [ConfigurationKeyName("log-ips")]
    public bool LogIps { get; set; } = true;

    [ConfigurationKeyName("enable-query")]
    public bool EnableQuery { get; set; } = false;

    [ConfigurationKeyName("query.port")]
    public ushort QueryPort { get; set; } = 25565;

    [ConfigurationKeyName("enable-rcon")]
    public bool EnableRcon { get; set; } = false;

    [ConfigurationKeyName("rcon.port")]
    public ushort RconPort { get; set; } = 25575;

    [ConfigurationKeyName("rcon.password")]
    public string RconPassword { get; set; } = string.Empty;

    #endregion

}
