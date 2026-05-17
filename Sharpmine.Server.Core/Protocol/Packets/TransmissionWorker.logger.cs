using Microsoft.Extensions.Logging;

namespace Sharpmine.Server.Core.Protocol.Packets;

public partial class TransmissionWorker
{

    [LoggerMessage(LogLevel.Error, "{Packet} has no implementation for serialization")]
    partial void LogSerializeNotImplemented(IClientboundPacket packet);

    [LoggerMessage(LogLevel.Error, "An error occurred while transmitting {Packet}")]
    partial void LogErrorWhileTransmittingPacket(Exception ex, IClientboundPacket? packet);

    [LoggerMessage(LogLevel.Debug, "Transmitting {Length} bytes: {Packet}")]
    partial void LogTransmittingPacket(IClientboundPacket packet, int length);

}
