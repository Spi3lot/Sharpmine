using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Sharpmine.Domain.Registries.Dynamic;

public class DynamicRegistry<T>
{

    private FrozenDictionary<Identifier, T> _entries = FrozenDictionary<Identifier, T>.Empty;

    public void Load(IDictionary<Identifier, T> entries)
    {
        _entries = entries.ToFrozenDictionary();
    }

    public T this[Identifier id] => _entries[id];

    public T? GetOrDefault(Identifier id) => _entries.GetValueOrDefault(id);

    public bool TryGet(Identifier id, [NotNullWhen(true)] out T? value) => _entries.TryGetValue(id, out value);

    public ImmutableArray<T> All => _entries.Values;

}
