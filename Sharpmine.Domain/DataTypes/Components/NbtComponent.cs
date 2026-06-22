using Raspite.Tags;

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

    public override ITag ToNbt(string name = "")
    {
        List<ITag> tags = [new StringTag(Nbt, "nbt")];
        if (Source is not null) tags.Add(new StringTag(Source, "source"));
        if (Interpret is not null) tags.Add(new ByteTag((byte) (Interpret.Value ? 1 : 0), "interpret"));
        if (Plain is not null) tags.Add(new ByteTag((byte) (Plain.Value ? 1 : 0), "plain"));
        if (Separator is not null) tags.Add(Separator.ToNbt("separator"));
        if (Entity is not null) tags.Add(new StringTag(Entity, "entity"));
        if (Block is not null) tags.Add(new StringTag(Block, "block"));
        if (Storage is not null) tags.Add(new StringTag(Storage, "storage"));
        ApplyStyleToNbt(tags);
        return new CompoundTag([.. tags], name);
    }

}
