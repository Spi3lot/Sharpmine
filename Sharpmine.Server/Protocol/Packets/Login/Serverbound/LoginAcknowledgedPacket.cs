namespace Sharpmine.Server.Protocol.Packets.Login.Serverbound;

public partial record LoginAcknowledgedPacket : IStateTransition, IHandlerless
{

    public ProtocolState NextState => ProtocolState.Configuration;

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader) => true;

}
