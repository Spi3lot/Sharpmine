using System.Net.Sockets;

using Microsoft.Extensions.Logging;

namespace Sharpmine.Server.Protocol.Packets;

public partial class PacketSender
{

    private readonly ILogger<PacketSender> _logger;

    private readonly MemoryStream _memoryStream;

    private readonly BinaryWriter _memoryStreamWriter;

    public PacketSender(ILogger<PacketSender> logger)
    {
        _logger = logger;
        _memoryStream = new MemoryStream();
        _memoryStreamWriter = new BinaryWriter(_memoryStream);
    }

    public async Task SendAsync(IClientboundPacket packet, NetworkStream stream, BinaryWriter writer)
    {
        _memoryStream.SetLength(0);
        _memoryStreamWriter.Write7BitEncodedInt(packet.Id);
        await packet.SerializeContentAsync(stream, writer);

        int packetLength = (int) _memoryStream.Length;
        writer.Write7BitEncodedInt(packetLength);
        writer.Write(_memoryStream.GetBuffer(), 0, packetLength);
        LogSentPacket(packet, packet.State, packet.Id, packetLength);
    }

    [LoggerMessage(LogLevel.Debug, "Sent {State}:0x{Id:X2} with {Length} bytes: {Packet}")]
    partial void LogSentPacket(IClientboundPacket packet, ProtocolState state, int id, int length);

}
