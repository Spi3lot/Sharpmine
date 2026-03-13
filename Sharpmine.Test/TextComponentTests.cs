using Sharpmine.Server.Protocol.Models;

using System.Text.Json;

namespace Sharpmine.Test;

public class TextComponentTests
{

    private const string SerializedLiteral = "\"Hello!\"";

    private const string SerializedList = """["Root","Extra1","Extra2"]""";

    private const string SerializedComplex = """{"text":"Complex","bold":true,"italic":true,"type":"Text","extra":["Literal", {"text":"Complex","bold":true,"italic":true,"type":"Text"}]}""";

    [Test]
    public void SerializeEmptyTest()
    {
        Assert.That(JsonSerializer.Serialize(new TextComponent()), Is.EqualTo("{}"));
    }

    [Test]
    public void SerializeLiteralTest()
    {
        var textComponent = TextComponent.Literal("Hello!");
        Assert.That(JsonSerializer.Serialize(textComponent), Is.EqualTo(SerializedLiteral));
    }

    [Test]
    public void SerializeListTest()
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
    public void SerializeComplexTest()
    {
        var textComponent = new TextComponent
        {
            Type = TextComponent.ContentType.Text,
            Text = "Complex",
            Bold = true,
            Italic = true,
            Extra = [TextComponent.Literal("Literal")]
        };

        Assert.That(
            JsonSerializer.Serialize(textComponent),
            Is.EqualTo(SerializedComplex)
        );
    }

    [Test]
    public void DeserializeLiteralTest()
    {
        Assert.Multiple(() =>
        {
            var textComponent = JsonSerializer.Deserialize<TextComponent>(SerializedLiteral);
            Assert.That(textComponent!.IsLiteral(), Is.True);
            Assert.That(textComponent.IsList(), Is.False);
            Assert.That(textComponent.AsLiteral(), Is.EqualTo("Hello!"));
        });
    }

    [Test]
    public void DeserializeListTest()
    {
        Assert.Multiple(() =>
        {
            var textComponent = JsonSerializer.Deserialize<TextComponent>(SerializedList);
            Assert.That(textComponent!.IsLiteral(), Is.False);
            Assert.That(textComponent.IsList(), Is.True);
            Assert.That(textComponent.Text!, Is.EqualTo("Root"));
            Assert.That(textComponent.Extra, Is.EquivalentTo([
                TextComponent.Literal("Extra1"),
                TextComponent.Literal("Extra2"),
            ]));
        });
    }

    [Test]
    public void DeserializeComplexTest()
    {
        Assert.Multiple(() =>
        {
            var textComponent = JsonSerializer.Deserialize<TextComponent>(SerializedComplex);
            Assert.That(textComponent!.IsLiteral(), Is.False);
            Assert.That(textComponent.IsList(), Is.True);
            Assert.That(textComponent.Text!, Is.EqualTo("Complex"));
            Assert.That(textComponent.Extra, Is.EquivalentTo([
                TextComponent.Literal("Literal"),
                textComponent with { Extra = null },
            ]));
        });
    }

}