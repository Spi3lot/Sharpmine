using System.Net;

using Microsoft.Extensions.Logging;

namespace Sharpmine.Server;

public partial class ServerService
{

    [LoggerMessage(LogLevel.Error, "An error occurred while handling client {EndPoint}")]
    partial void LogErrorWhileHandling(Exception exception, EndPoint? endPoint);

    [LoggerMessage(LogLevel.Error, "An error occurred while trying to accept a client")]
    partial void LogErrorWhileAccepting(Exception ex);

    [LoggerMessage(LogLevel.Information, "Started server, listening on port {Port}")]
    partial void LogStartedServer(int port);

    [LoggerMessage(LogLevel.Information, "Stopping server...")]
    partial void LogStoppingServer();

}
