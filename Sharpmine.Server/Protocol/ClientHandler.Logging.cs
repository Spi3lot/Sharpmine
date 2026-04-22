using Microsoft.Extensions.Logging;

using Sharpmine.Server.Protocol.Packets;

namespace Sharpmine.Server.Protocol;

public partial class ClientHandler
{

    [LoggerMessage(LogLevel.Error, "{Packet} has no implementation for processing")]
    partial void LogProcessNotImplemented(IServerboundPacket packet);

    [LoggerMessage(LogLevel.Error, "An error occurred while handling the client")]
    partial void LogErrorWhileHandling(Exception error);

    [LoggerMessage(LogLevel.Information, "{Handler} connected")]
    partial void LogClientConnected(ClientHandler handler);

    [LoggerMessage(LogLevel.Information, "{Handler} disconnected")]
    partial void LogClientDisconnected(ClientHandler handler);

    [LoggerMessage(LogLevel.Information, "Connection to {Handler} was closed by server: {Reason}")]
    partial void LogClientWasDisconnected(ClientHandler handler, string reason);

    [LoggerMessage(LogLevel.Debug, "Transitioning from state {OldState} to {NewState}")]
    partial void LogStateTransition(ProtocolState oldState, ProtocolState newState);

}
