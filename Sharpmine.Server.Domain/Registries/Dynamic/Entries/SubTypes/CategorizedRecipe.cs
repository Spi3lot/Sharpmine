namespace Sharpmine.Server.Domain.Registries.Dynamic.Entries.SubTypes;

public abstract record CategorizedRecipe : Recipe
{

    public required string Category { get; init; }

}
