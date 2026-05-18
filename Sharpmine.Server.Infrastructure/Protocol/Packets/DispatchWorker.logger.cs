using Microsoft.Extensions.Logging;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets;

public partial class DispatchWorker
{

    [LoggerMessage(LogLevel.Error, "An error occurred while handling {Packet}")]
    partial void LogErrorWhileHandlingPacket(Exception ex, IServerboundPacket? packet);

    [LoggerMessage(LogLevel.Warning, "Missing handler for {Packet}")]
    partial void LogNoPacketHandler(IServerboundPacket packet);

}
