namespace Sharpmine.Server.Core.Protocol;

public interface IStateTransition
{

    ProtocolState NextState { get; }

}
