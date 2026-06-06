using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Components;

public sealed record ScoreComponent(Score Score) : Component
{

    public override Tag ToNbt(string name = "")
    {
        List<Tag> tags = [Score.ToNbt("score")]; 
        ApplyStyleToNbt(tags);
        return new CompoundTag([.. tags], name);
    }

}
