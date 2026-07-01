using Raspite.Tags;
using Raspite.Tags.Building;

namespace Sharpmine.Domain.DataTypes.Components;

public sealed record NbtComponent(
    string Nbt,
    string? Source = null,
    bool? Interpret = null,
    bool? Plain = null,
    Component? Separator = null,
    string? Entity = null,
    string? Block = null,
    string? Storage = null) : Component
{

    public override CompoundTagBuilder ToCompoundNbtBuilder(string name = "")
    {
        return base.ToCompoundNbtBuilder(name)
            .AddString(Nbt, "nbt")
            .AddString(Source, "source")
            .AddBoolean(Interpret, "interpret")
            .AddBoolean(Plain, "plain")
            .Add(Separator?.ToNbt("separator"))
            .AddString(Entity, "entity")
            .AddString(Block, "block")
            .AddString(Storage, "storage");
    }

}
