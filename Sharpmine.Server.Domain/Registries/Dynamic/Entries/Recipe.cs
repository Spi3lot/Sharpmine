using System.Text.Json.Serialization;

using Sharpmine.Domain;

namespace Sharpmine.Server.Domain.Registries.Dynamic.Entries;

[JsonConverter(typeof(RecipeConverter))]
public abstract record Recipe
{

    public required Identifier Type { get; init; }

}
