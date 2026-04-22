namespace Sharpmine.Server.Protocol.Packets.Login.Serverbound;

public partial record LoginAcknowledgedPacket : IStateTransition
{

    public ProtocolState NextState => ProtocolState.Configuration;

    public ProtocolResult DeserializeContent(NetworkStream stream, BinaryReader reader) => ProtocolResult.Success;

    public ValueTask<ProtocolResult> ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(ProtocolResult.Success);
    }

}
