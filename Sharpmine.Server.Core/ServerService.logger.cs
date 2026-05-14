using Microsoft.Extensions.Logging;

namespace Sharpmine.Server.Core;

public partial class ServerService
{

    [LoggerMessage(LogLevel.Error, "An error occurred while trying to accept a client")]
    partial void LogErrorWhileAccepting(Exception ex);

    [LoggerMessage(LogLevel.Error, "An error occurred while handling client {Ip}")]
    partial void LogErrorWhileHandling(Exception exception, string ip);

    [LoggerMessage(LogLevel.Warning, "Could not determine IP of newly connected client")]
    partial void LogClientIpIndeterminable();

    [LoggerMessage(LogLevel.Debug, "Blacklisted IP {Ip} attempted to connect")]
    partial void LogClientBlacklisted(string ip);

    [LoggerMessage(LogLevel.Information, "Started server, listening on port {Port}")]
    partial void LogStartedServer(int port);

    [LoggerMessage(LogLevel.Information, "Stopping server...")]
    partial void LogStoppingServer();

}
