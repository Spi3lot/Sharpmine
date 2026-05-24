using System.Collections.Immutable;

namespace Sharpmine.Domain.Tags;

public interface ITagProvider : IProvider<ImmutableArray<TaggedRegistryData>>;
