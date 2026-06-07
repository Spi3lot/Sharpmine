namespace Sharpmine.Server.Domain.Registries.Dynamic.Entries.SubTypes;

public abstract record StandardRecipe : CategorizedRecipe
{

    public string? Group { get; init; }

}
