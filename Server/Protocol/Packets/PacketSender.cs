using System.Net.Sockets;

namespace Sharpmine.Server.Protocol.Packets;

public class PacketSender
{

    private readonly MemoryStream _memoryStream;

    private readonly BinaryWriter _memoryStreamWriter;

    public PacketSender()
    {
        _memoryStream = new MemoryStream();
        _memoryStreamWriter = new BinaryWriter(_memoryStream);
    }

    public async Task SendAsync(IClientboundPacket packet, NetworkStream stream, BinaryWriter writer)
    {
        _memoryStream.SetLength(0);
        _memoryStreamWriter.Write7BitEncodedInt(packet.Id);
        await packet.SerializeAsync(stream, writer);

        int packetLength = (int) _memoryStream.Length;
        writer.Write7BitEncodedInt(packetLength);
        writer.Write(_memoryStream.GetBuffer(), 0, packetLength);
    }

}