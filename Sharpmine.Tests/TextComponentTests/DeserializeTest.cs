using System.Text.Json;

using Sharpmine.Server.Protocol.DataTypes;

using static Sharpmine.Tests.TextComponentTests.ITextComponentTest;

namespace Sharpmine.Tests.TextComponentTests;

public class DeserializeTest : ITextComponentTest
{

    [Test]
    public async Task Empty()
    {
        await Assert.That(JsonSerializer.Deserialize<TextComponent>("{}"))
            .IsEqualTo(new TextComponent());
    }

    [Test]
    public async Task Literal()
    {
        var actual = JsonSerializer.Deserialize<TextComponent>(SerializedLiteral);

        using (Assert.Multiple())
        {
            await Assert.That(actual).IsNotNull();
            await Assert.That(actual!.IsLiteral()).IsTrue();
            await Assert.That(actual.IsList()).IsFalse();
            await Assert.That(actual.AsLiteral()).IsEqualTo("Hello!");
        }
    }

    [Test]
    public async Task List()
    {
        var actual = JsonSerializer.Deserialize<TextComponent>(SerializedList);

        using (Assert.Multiple())
        {
            await Assert.That(actual).IsNotNull();
            await Assert.That(actual!.IsLiteral()).IsFalse();
            await Assert.That(actual.IsList()).IsTrue();
            await Assert.That(actual.Text).IsEqualTo("Root");

            await Assert.That(actual.Extra).IsEquivalentTo([
                TextComponent.Literal("Extra1"),
                TextComponent.Literal("Extra2")
            ]);
        }
    }

    [Test]
    public async Task Complex()
    {
        var actual = JsonSerializer.Deserialize<TextComponent>(SerializedComplexAsObject);

        var expected = new TextComponent
        {
            Type = TextComponent.ContentType.Text,
            Text = "Complex",
            Bold = true,
            Italic = true,
            Extra = [TextComponent.Literal("Literal")],
        };

        expected.Extra.Add(expected with { Extra = null });

        using (Assert.Multiple())
        {
            await Assert.That(actual).IsNotNull();
            await Assert.That(actual!.IsLiteral()).IsFalse();
            await Assert.That(actual.IsList()).IsTrue();
            await Assert.That(actual with { Extra = null }).IsEqualTo(expected with { Extra = null });
            await Assert.That(actual.Extra).IsEquivalentTo(expected.Extra);
        }
    }

    [Test]
    public async Task ShadowColor()
    {
        var expected = new TextComponent { ShadowColor = 0x72786125 };

        using (Assert.Multiple())
        {
            await Assert.That(JsonSerializer.Deserialize<TextComponent>("""{"shadow_color":1920491813}"""))
                .IsEqualTo(expected);

            await Assert.That(JsonSerializer.Deserialize<TextComponent>("""{"shadow_color":[0.470,0.380,0.145,0.447]}"""))
                .IsEqualTo(expected);

            await Assert.That(() => JsonSerializer.Deserialize<TextComponent>("""{"shadow_color":[0.12652, 0.6981, 1]}"""))
                .ThrowsAsync<TextComponent, JsonException>();

            await Assert.That(() => JsonSerializer.Deserialize<TextComponent>("""{"shadow_color":[0, 0.6981, 1, 5, 6]}"""))
                .ThrowsAsync<TextComponent, JsonException>();

            await Assert.That(() => JsonSerializer.Deserialize<TextComponent>("""{"shadow_color":[0, 0.6981, 1, 4}"""))
                .ThrowsAsync<TextComponent, JsonException>();

            await Assert.That(() => JsonSerializer.Deserialize<TextComponent>("""{"shadow_color":[0, 0.6981, 1, 4,"""))
                .ThrowsAsync<TextComponent, JsonException>();
        }
    }

}
