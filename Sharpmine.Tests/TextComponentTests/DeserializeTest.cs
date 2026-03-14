using System.Text.Json;

using Sharpmine.Server.Protocol.Models;

using static Sharpmine.Tests.TextComponentTests.ITextComponentTest;

namespace Sharpmine.Tests.TextComponentTests;

public class DeserializeTest : ITextComponentTest
{

    [Test]
    public void Empty()
    {
        Assert.That(
            JsonSerializer.Deserialize<TextComponent>("{}"),
            Is.EqualTo(new TextComponent())
        );
    }

    [Test]
    public void Literal()
    {
        var actual = JsonSerializer.Deserialize<TextComponent>(SerializedLiteral);

        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual!.IsLiteral(), Is.True);
            Assert.That(actual.IsList(), Is.False);
            Assert.That(actual.AsLiteral(), Is.EqualTo("Hello!"));
        });
    }

    [Test]
    public void List()
    {
        var actual = JsonSerializer.Deserialize<TextComponent>(SerializedList);

        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual!.IsLiteral(), Is.False);
            Assert.That(actual.IsList(), Is.True);
            Assert.That(actual.Text, Is.EqualTo("Root"));

            Assert.That(actual.Extra, Is.EqualTo([
                TextComponent.Literal("Extra1"),
                TextComponent.Literal("Extra2")
            ]));
        });
    }

    [Test]
    public void Complex()
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

        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual!.IsLiteral(), Is.False);
            Assert.That(actual.IsList(), Is.True);
            Assert.That(actual with { Extra = null }, Is.EqualTo(expected with { Extra = null }));
            Assert.That(actual.Extra, Is.EqualTo(expected.Extra));
        });
    }

    [Test]
    public void ShadowColor()
    {
        var expected = new TextComponent { ShadowColor = 0x72786125 };

        Assert.Multiple(() =>
        {
            Assert.That(
                JsonSerializer.Deserialize<TextComponent>("""{"shadow_color":1920491813}"""),
                Is.EqualTo(expected)
            );

            Assert.That(
                JsonSerializer.Deserialize<TextComponent>("""{"shadow_color":[0.470,0.380,0.145,0.447]}"""),
                Is.EqualTo(expected)
            );

            Assert.That(
                () => JsonSerializer.Deserialize<TextComponent>("""{"shadow_color":[0.12652, 0.6981, 1]}"""),
                Throws.TypeOf<JsonException>()
            );

            Assert.That(
                () => JsonSerializer.Deserialize<TextComponent>("""{"shadow_color":[0, 0.6981, 1, 5, 6]}"""),
                Throws.TypeOf<JsonException>()
            );

            Assert.That(
                () => JsonSerializer.Deserialize<TextComponent>("""{"shadow_color":[0, 0.6981, 1, 4}"""),
                Throws.TypeOf<JsonException>()
            );

            Assert.That(
                () => JsonSerializer.Deserialize<TextComponent>("""{"shadow_color":[0, 0.6981, 1, 4,"""),
                Throws.TypeOf<JsonException>()
            );
        });
    }

}