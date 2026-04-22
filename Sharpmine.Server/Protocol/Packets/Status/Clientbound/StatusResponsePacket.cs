using Sharpmine.Server.Protocol.Extensions;

namespace Sharpmine.Server.Protocol.Packets.Status.Clientbound;

public partial record StatusResponsePacket(ServerStatus Status)
{

    public void SerializeContent(Stream stream, BinaryWriter writer)
    {
        var memoryStream = new MemoryStream();
        memoryStream.WriteJson(Status);
        short length = checked((short) memoryStream.Length);
        writer.Write7BitEncodedInt(length);
        stream.Write(memoryStream.GetBuffer(), 0, length);
    }

}
