using Sharpmine.Server.Protocol.Extensions;

namespace Sharpmine.Server.Protocol.Packets.Login.Serverbound;

public partial record HelloPacket
{

    public string Name { get; set; } = null!;

    public Guid Uuid { get; set; }

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        Name = reader.ReadString();
        Uuid = reader.ReadUuid();
        return true;
    }

}
