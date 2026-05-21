using System.Collections.Immutable;

namespace Sharpmine.Server.Infrastructure.Protocol.Versions;

public interface IProtocol
{

    int VersionNumber { get; }

    string ReleaseName { get; }

    ImmutableArray<string> SynchronizedRegistryIds { get; }

}
