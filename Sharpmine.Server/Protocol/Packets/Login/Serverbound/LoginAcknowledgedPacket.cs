namespace Sharpmine.Server.Protocol.Packets.Login.Serverbound;

public partial record LoginAcknowledgedPacket : IStateTransition, IUnhandledPacket
{

    public ProtocolState NextState => ProtocolState.Configuration;

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader) => true;

}
