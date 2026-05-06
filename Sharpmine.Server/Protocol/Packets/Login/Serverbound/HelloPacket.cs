using Optional;

using Sharpmine.Server.Protocol.DataTypes;
using Sharpmine.Server.Protocol.Packets.Login.Clientbound;
using Sharpmine.Server.Security;

namespace Sharpmine.Server.Protocol.Packets.Login.Serverbound;

public partial record HelloPacket
{

    public string Name { get; set; } = null!;

    public Guid Uuid { get; set; }

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        Name = reader.ReadString();
        Uuid = new Guid(reader.ReadBytes(16));
        return true;
    }

}
