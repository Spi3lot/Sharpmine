namespace Sharpmine.Server.Packets.Clientbound;

public class PacketSerializer
{

    private readonly MemoryStream _memoryStream;

    private readonly BinaryWriter _memoryStreamWriter;

    public PacketSerializer()
    {
        _memoryStream = new MemoryStream();
        _memoryStreamWriter = new BinaryWriter(_memoryStream);
    }

    public void Serialize(IClientboundPacket packet, BinaryWriter writer)
    {
        _memoryStream.SetLength(0);
        _memoryStreamWriter.Write7BitEncodedInt(packet.Id);
        packet.Serialize(writer);
        
        int packetLength = (int) _memoryStream.Length;
        writer.Write7BitEncodedInt(packetLength);
        writer.Write(_memoryStream.GetBuffer(),  0, packetLength);
    }

}