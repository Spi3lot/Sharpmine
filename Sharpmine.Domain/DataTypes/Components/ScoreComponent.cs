using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Components;

public sealed record ScoreComponent(Score Score) : Component
{

    public override ITag ToNbt(string name = "")
    {
        List<ITag> tags = [Score.ToNbt("score")]; 
        ApplyStyleToNbt(tags);
        return new CompoundTag([.. tags], name);
    }

}
