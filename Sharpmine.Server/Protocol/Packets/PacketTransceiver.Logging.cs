using Microsoft.Extensions.Logging;

namespace Sharpmine.Server.Protocol.Packets;

public partial class PacketTransceiver
{

    [LoggerMessage(LogLevel.Error, "{Packet} has no implementation for serialization")]
    partial void LogSerializeNotImplemented(IClientboundPacket packet);

    [LoggerMessage(LogLevel.Error, "Deserializing {Packet} caused a violation")]
    partial void LogDeserializeViolation(IServerboundPacket packet);

    [LoggerMessage(LogLevel.Error, "{Packet} has no implementation for deserialization")]
    partial void LogDeserializeNotImplemented(IServerboundPacket packet);

    [LoggerMessage(LogLevel.Error, "Received a packet with an alleged length of 0 in state {State}")]
    partial void LogReceivedEmptyPacket(ProtocolState state);

    [LoggerMessage(LogLevel.Error, "Received {State}:0x{Id:X2} with {Length} bytes: UNKNOWN PACKET")]
    partial void LogReceivedUnknownPacket(ProtocolState state, int id, int length);

    [LoggerMessage(LogLevel.Warning, "Received legacy ping, closing connection")]
    partial void LogReceivedLegacyPing();

    [LoggerMessage(LogLevel.Debug, "Transmitted {State}:0x{Id:X2} with {Length} bytes: {Packet}")]
    partial void LogTransmittedPacket(IClientboundPacket packet, ProtocolState state, int id, int length);

    [LoggerMessage(LogLevel.Debug, "Received {State}:0x{Id:X2} with {Length} bytes: {Packet}")]
    partial void LogReceivedPacket(IServerboundPacket packet, ProtocolState state, int id, int length);

}
