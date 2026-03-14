using System.Text.Json;

using Sharpmine.Server.Protocol.Models;

using static Sharpmine.Tests.TextComponentTests.ITextComponentTest;

namespace Sharpmine.Tests.TextComponentTests;

public class SerializeTest : ITextComponentTest
{

    [Test]
    public void Empty()
    {
        Assert.That(
            JsonSerializer.Serialize(new TextComponent()),
            Is.EqualTo("{}")
        );
    }

    [Test]
    public void Literal()
    {
        var textComponent = TextComponent.Literal("Hello!");
        Assert.That(JsonSerializer.Serialize(textComponent), Is.EqualTo(SerializedLiteral));
    }

    [Test]
    public void List()
    {
        List<TextComponent> componentList =
        [
            TextComponent.Literal("Root"),
            TextComponent.Literal("Extra1"),
            TextComponent.Literal("Extra2")
        ];

        var listComponent = TextComponent.List(componentList);

        Assert.Multiple(() =>
        {
            Assert.That(
                JsonSerializer.Serialize(listComponent),
                Is.EqualTo(SerializedList)
            );

            Assert.That(
                JsonSerializer.Serialize(componentList),
                Is.EqualTo(SerializedList)
            );
        });
    }

    [Test]
    public void Complex()
    {
        var textComponent = new TextComponent
        {
            Type = TextComponent.ContentType.Text,
            Text = "Complex",
            Bold = true,
            Italic = true,
            Extra = [TextComponent.Literal("Literal")],
        };

        textComponent.Extra.Add(textComponent with { Extra = null });

        Assert.That(
            JsonSerializer.Serialize(textComponent),
            Is.EqualTo(SerializedComplexAsList)
        );
    }

    [Test]
    public void ShadowColor()
    {
        var textComponent = new TextComponent { ShadowColor = 0x72786125 };

        Assert.That(
            JsonSerializer.Serialize(textComponent),
            Is.EqualTo("""{"shadow_color":1920491813}""")
        );
    }

}