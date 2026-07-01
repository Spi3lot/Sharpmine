using Raspite.Tags.Building;

namespace Sharpmine.Domain.DataTypes.Components;

public sealed record ScoreComponent(Score Score) : Component
{

    public override CompoundTagBuilder ToCompoundNbtBuilder(string name = "")
    {
        return base.ToCompoundNbtBuilder(name)
            .Add(Score.ToNbt("score"));
    }

}
