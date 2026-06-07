using System.Text.Json;

using Sharpmine.Domain.DataTypes.Components;

using static Sharpmine.Tests.TextComponentTests.ITextComponentTest;

namespace Sharpmine.Tests.TextComponentTests;

public class SerializeTest : ITextComponentTest
{

    [Test]
    public async Task Empty()
    {
        var textComponent = new TextComponent(string.Empty);

        await Assert.That(JsonSerializer.Serialize<Component>(textComponent))
            .IsEqualTo("\"\"");
    }

    [Test]
    public async Task Literal()
    {
        var textComponent = new TextComponent("Hello!");

        await Assert.That(JsonSerializer.Serialize<Component>(textComponent))
            .IsEqualTo(SerializedLiteral);
    }

    [Test]
    public async Task List()
    {
        List<Component> componentList =
        [
            new TextComponent("Root"),
            new TextComponent("Extra1"),
            new TextComponent("Extra2")
        ];

        var listComponent = Component.List(componentList);

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
        var textComponent = new TextComponent("Complex")
        {
            Style = new ComponentStyle
            {
                Bold = true,
                Italic = true,
            },
            Extra = [new TextComponent("Literal")],
        };

        textComponent.Extra.Add(textComponent with { Extra = null });

        await Assert.That(JsonSerializer.Serialize<Component>(textComponent))
            .IsEqualTo(SerializedComplexAsList);
    }

    [Test]
    public async Task ShadowColor()
    {
        var textComponent = new TextComponent(string.Empty) { Style = new ComponentStyle { ShadowColor = 0x72786125 } };

        await Assert.That(JsonSerializer.Serialize<Component>(textComponent))
            .IsEqualTo("""{"text":"","shadow_color":1920491813}""");
    }

}
