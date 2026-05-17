using Microsoft.Extensions.Logging;

namespace Sharpmine.Server.Core.Protocol.Packets;

public partial class TransmissionWorker
{

    [LoggerMessage(LogLevel.Error, "An error occurred while transmitting {Packet}")]
    public partial void LogErrorWhileTransmittingPacket(Exception ex, IClientboundPacket? packet);

}
