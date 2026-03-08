namespace Sharpmine.Server.Packets.Clientbound;

public interface IClientboundPacket
{
    
    int Id { get; }

    Task Serialize(BinaryWriter writer);

}