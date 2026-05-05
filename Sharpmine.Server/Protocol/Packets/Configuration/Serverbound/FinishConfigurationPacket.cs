namespace Sharpmine.Server.Protocol.Packets.Configuration.Serverbound;

public partial record FinishConfigurationPacket : IStateTransition, IUnhandledPacket
{

    public ProtocolState NextState => ProtocolState.Play;

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader) => true;

}
