using System.Collections.Immutable;

namespace Sharpmine.Domain.Tags;

public interface ITagLoader
{

    ImmutableArray<TaggedRegistryData> Load();

}
