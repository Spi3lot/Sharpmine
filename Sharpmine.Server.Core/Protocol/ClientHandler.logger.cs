using Microsoft.Extensions.Logging;

using Sharpmine.Server.Core.Protocol.Packets;

namespace Sharpmine.Server.Core.Protocol;

public partial class ClientHandler
{

    [LoggerMessage(LogLevel.Error, "Attempted to send {Packet} while in state {State}")]
    partial void LogUnmatchedStates(IClientboundPacket packet, ProtocolState state);

    [LoggerMessage(LogLevel.Error, "An error occurred while handling the client")]
    partial void LogErrorWhileHandling(Exception error);

    [LoggerMessage(LogLevel.Error, "An error occurred while transmitting {Packet}")]
    public partial void LogErrorWhileTransmittingPacket(Exception ex, IClientboundPacket? packet);

    [LoggerMessage(LogLevel.Error, "An error occurred while handling {Packet}")]
    public partial void LogErrorWhileHandlingPacket(Exception ex, IServerboundPacket? packet);

    [LoggerMessage(LogLevel.Warning, "Missing handler for {Packet}")]
    public partial void LogNoPacketHandler(IServerboundPacket packet);

    [LoggerMessage(LogLevel.Warning, "Disconnect requested in state {State}, aborting forcefully.")]
    partial void LogDisconnectRequestedInInvalidState(Exception ex, ProtocolState state);

    [LoggerMessage(LogLevel.Information, "{Handler} connected")]
    partial void LogClientConnected(ClientHandler handler);

    [LoggerMessage(LogLevel.Information, "{Handler} disconnected")]
    partial void LogClientDisconnected(ClientHandler handler);

    [LoggerMessage(LogLevel.Information, "Connection to {Handler} was closed by server: {Reason}")]
    partial void LogDisconnectingClient(ClientHandler handler, string reason);

    [LoggerMessage(LogLevel.Debug, "Transitioning from state {OldState} to {NewState}")]
    partial void LogStateTransition(ProtocolState oldState, ProtocolState newState);

}
