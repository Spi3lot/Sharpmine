using System.Text.Json;

using Sharpmine.Server.Protocol.DataTypes;

using static Sharpmine.Tests.TextComponentTests.ITextComponentTest;

namespace Sharpmine.Tests.TextComponentTests;

public class SerializeTest : ITextComponentTest
{

    [Test]
    public async Task Empty()
    {
        var textComponent = new TextComponent();

        await Assert.That(JsonSerializer.Serialize(textComponent))
            .IsEqualTo("{}");
    }

    [Test]
    public async Task Literal()
    {
        var textComponent = TextComponent.Literal("Hello!");

        await Assert.That(JsonSerializer.Serialize(textComponent))
            .IsEqualTo(SerializedLiteral);
    }

    [Test]
    public async Task List()
    {
        List<TextComponent> componentList =
        [
            TextComponent.Literal("Root"),
            TextComponent.Literal("Extra1"),
            TextComponent.Literal("Extra2")
        ];

        var listComponent = TextComponent.List(componentList);

        using (Assert.Multiple())
        {
            await Assert.That(JsonSerializer.Serialize(listComponent))
                .IsEqualTo(SerializedList);

            await Assert.That(JsonSerializer.Serialize(componentList))
                .IsEqualTo(SerializedList);
        }
    }

    [Test]
    public async Task Complex()
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

        await Assert.That(JsonSerializer.Serialize(textComponent))
            .IsEqualTo(SerializedComplexAsList);
    }

    [Test]
    public async Task ShadowColor()
    {
        var textComponent = new TextComponent { ShadowColor = 0x72786125 };

        await Assert.That(JsonSerializer.Serialize(textComponent))
            .IsEqualTo("""{"shadow_color":1920491813}""");
    }

}
