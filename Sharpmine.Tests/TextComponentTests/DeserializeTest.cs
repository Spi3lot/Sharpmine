using System.Text.Json;

using Sharpmine.Domain.DataTypes.Components;

using static Sharpmine.Tests.TextComponentTests.ITextComponentTest;

namespace Sharpmine.Tests.TextComponentTests;

public class DeserializeTest : ITextComponentTest
{

    [Test]
    public async Task Empty()
    {
        await Assert.That(JsonSerializer.Deserialize<Component>("""{"text":""}"""))
            .IsEqualTo(new TextComponent(string.Empty));
    }

    [Test]
    public async Task Literal()
    {
        var actual = JsonSerializer.Deserialize<Component>(SerializedLiteral);

        using (Assert.Multiple())
        {
            await Assert.That(actual).IsNotNull();
            await Assert.That(actual is TextComponent).IsTrue();
            await Assert.That(actual.IsList).IsFalse();
            await Assert.That(actual.Style.IsEmpty).IsTrue();
            await Assert.That(((TextComponent) actual).Text).IsEqualTo("Hello!");
        }
    }

    [Test]
    public async Task List()
    {
        var actual = JsonSerializer.Deserialize<Component>(SerializedList);

        using (Assert.Multiple())
        {
            await Assert.That(actual).IsNotNull();
            await Assert.That(actual is TextComponent { IsPlain: true }).IsFalse();
            await Assert.That(actual.IsList).IsTrue();
            await Assert.That(((TextComponent) actual).Text).IsEqualTo("Root");

            await Assert.That(actual.Extra).IsEquivalentTo(new[]
            {
                new TextComponent("Extra1"),
                new TextComponent("Extra2")
            });
        }
    }

    [Test]
    public async Task Complex()
    {
        var actual = JsonSerializer.Deserialize<Component>(SerializedComplexAsObject);

        var expected = new TextComponent("Complex")
        {
            Style = new ComponentStyle
            {
                Bold = true,
                Italic = true,
            },
            Extra = [new TextComponent("Literal")],
        };

        expected.Extra.Add(expected with { Extra = null });

        using (Assert.Multiple())
        {
            await Assert.That(actual).IsNotNull();
            await Assert.That(actual is TextComponent { IsPlain: true }).IsFalse();
            await Assert.That(actual.IsList).IsTrue();
            await Assert.That(actual with { Extra = null }).IsEqualTo(expected with { Extra = null });
            await Assert.That(actual.Extra).IsEquivalentTo(expected.Extra);
        }
    }

    [Test]
    public async Task ShadowColor()
    {
        var expected = new TextComponent(string.Empty) { Style = new ComponentStyle { ShadowColor = 0x72786125 } };

        using (Assert.Multiple())
        {
            await Assert.That(JsonSerializer.Deserialize<Component>("""{"shadow_color":1920491813}"""))
                .IsEqualTo(expected);

            await Assert.That(JsonSerializer.Deserialize<Component>("""{"shadow_color":[0.470,0.380,0.145,0.447]}"""))
                .IsEqualTo(expected);

            await Assert.That(() => JsonSerializer.Deserialize<Component>("""{"shadow_color":[0.12652, 0.6981, 1]}"""))
                .ThrowsAsync<Component, JsonException>();

            await Assert.That(() => JsonSerializer.Deserialize<Component>("""{"shadow_color":[0, 0.6981, 1, 5, 6]}"""))
                .ThrowsAsync<Component, JsonException>();

            await Assert.That(() => JsonSerializer.Deserialize<Component>("""{"shadow_color":[0, 0.6981, 1, 4}"""))
                .ThrowsAsync<Component, JsonException>();

            await Assert.That(() => JsonSerializer.Deserialize<Component>("""{"shadow_color":[0, 0.6981, 1, 4,"""))
                .ThrowsAsync<Component, JsonException>();
        }
    }

}
