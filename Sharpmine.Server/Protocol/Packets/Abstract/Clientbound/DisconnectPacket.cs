namespace Sharpmine.Server.Protocol.Packets.Abstract.Clientbound;

public abstract partial record DisconnectPacket
{

    public string Reason { get; init; } = null!;

    public void SerializeContent(Stream stream, BinaryWriter writer)
    {
        
        throw new NotImplementedException();
    }

}
