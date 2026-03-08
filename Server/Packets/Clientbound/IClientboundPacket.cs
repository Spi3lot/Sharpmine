namespace Sharpmine.Server.Packets.Clientbound;

public interface IClientboundPacket
{
    
    int Id { get; }

    void Serialize(BinaryWriter writer);

}