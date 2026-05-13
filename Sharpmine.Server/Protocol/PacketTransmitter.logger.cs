using Microsoft.Extensions.Logging;

using Sharpmine.Server.Protocol.Packets;

namespace Sharpmine.Server.Protocol;

public partial class PacketTransmitter
{

    [LoggerMessage(LogLevel.Error, "{Packet} has no implementation for serialization")]
    partial void LogSerializeNotImplemented(IClientboundPacket packet);

    [LoggerMessage(LogLevel.Debug, "Transmitted {State}:0x{Id:X2} with {Length} bytes: {Packet}")]
    partial void LogTransmittedPacket(IClientboundPacket packet, ProtocolState state, int id, int length);

}
