using System.Collections.Frozen;

namespace Sharpmine.Domain.Items;

public class Item(Identifier id, Dictionary<Identifier, object>? components = null)
{

    public Identifier Id { get; } = id;

    public FrozenDictionary<Identifier, object> Components { get; } = (components ?? []).ToFrozenDictionary();

    public T? GetComponent<T>(Identifier componentId)
    {
        return (Components.TryGetValue(componentId, out object? component) && component is T typedComponent)
            ? typedComponent
            : default;
    }

    public bool HasComponent(Identifier componentId)
    {
        return Components.ContainsKey(componentId);
    }

}
