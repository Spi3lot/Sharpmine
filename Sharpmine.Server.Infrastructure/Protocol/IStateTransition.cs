namespace Sharpmine.Server.Infrastructure.Protocol;

public interface IStateTransition
{

    ProtocolState NextState { get; }

}
