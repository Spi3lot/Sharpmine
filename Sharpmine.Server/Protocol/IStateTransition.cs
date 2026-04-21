namespace Sharpmine.Server.Protocol;

public interface IStateTransition
{

    ProtocolState NextState { get; }

}
