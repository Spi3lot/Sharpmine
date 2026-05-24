using System.Collections.Immutable;

using Sharpmine.Domain;

namespace Sharpmine.Server.Infrastructure.Protocol.Versions;

public interface IProtocol
{

    int VersionNumber { get; }

    string ReleaseName { get; }

    ImmutableArray<Identifier> SynchronizedRegistryIds { get; }

}
