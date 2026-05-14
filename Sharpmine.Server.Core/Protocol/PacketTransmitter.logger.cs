using Microsoft.Extensions.Logging;

using Sharpmine.Server.Core.Protocol.Packets;

namespace Sharpmine.Server.Core.Protocol;

public partial class PacketTransmitter
{

    [LoggerMessage(LogLevel.Error, "{Packet} has no implementation for serialization")]
    partial void LogSerializeNotImplemented(IClientboundPacket packet);

    [LoggerMessage(LogLevel.Debug, "Transmitting {State}:0x{Id:X2} with {Length} bytes: {Packet}")]
    partial void LogTransmittingPacket(IClientboundPacket packet, ProtocolState state, int id, int length);

}
