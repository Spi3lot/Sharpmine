using Microsoft.Extensions.Logging;

using Sharpmine.Server.Core.Protocol.Packets;

namespace Sharpmine.Server.Core.Protocol;

public partial class PacketReceiver
{

    [LoggerMessage(LogLevel.Error, "{Packet} has no implementation for deserialization")]
    partial void LogDeserializeNotImplemented(IServerboundPacket packet);

    [LoggerMessage(LogLevel.Warning, "Deserializing {Packet} caused a violation")]
    partial void LogDeserializeViolation(IServerboundPacket packet);

    [LoggerMessage(LogLevel.Warning, "Received corrupted packet length in state {State}")]
    partial void LogReceivedCorruptedPacketLength(ProtocolState state);

    [LoggerMessage(LogLevel.Warning, "Received corrupted packet ID for a packet {Length} bytes in length in state {State}")]
    partial void LogReceivedCorruptedPacketId(ProtocolState state, int length);

    [LoggerMessage(LogLevel.Warning, "Received a packet with an alleged length of 0 in state {State}")]
    partial void LogReceivedEmptyPacket(ProtocolState state);

    [LoggerMessage(LogLevel.Warning, "Received {State}:0x{Id:X2} with {Length} bytes: UNKNOWN PACKET")]
    partial void LogReceivedUnknownPacket(ProtocolState state, int id, int length);

    [LoggerMessage(LogLevel.Warning, "Received legacy ping, closing connection")]
    partial void LogReceivedLegacyPing();

    [LoggerMessage(LogLevel.Debug, "Received {Length} bytes: {Packet}")]
    partial void LogReceivedPacket(IServerboundPacket packet, int length);

    [LoggerMessage(LogLevel.Debug, "Pending pipe read was canceled")]
    partial void LogPendingPipeReadCanceled();

}
