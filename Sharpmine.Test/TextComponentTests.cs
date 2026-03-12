

using System.Text.Json;
using Sharpmine.Server.Protocol.Models;

namespace Sharpmine.Test;

public class TextComponentTests
{

    [Test]
    public void SerializeEmptyTest()
    {
        Assert.Throws<InvalidOperationException>(() => JsonSerializer.Serialize(new TextComponent()));
    }

    [Test]
    public void SerializeLiteralTest()
    {
        var textComponent = TextComponent.Literal("Hello!");
        Assert.Equals(() => JsonSerializer.Serialize(textComponent));
    }

    [Test]
    public void SerializeTest2()
    {
        var textComponent = TextComponent.List("Hello!");
        Assert.DoesNotThrow(() => JsonSerializer.Serialize(textComponent));
    }

    [Test]
    public void DeserializeTest()
    {
        Assert.Multiple(() =>
        {
            var textComponent = JsonSerializer.Deserialize<TextComponent>("""["Root", "Extra1", "Extra2"]""");
            Assert.Equals(textComponent.IsList, true);
            Assert.Equals(textComponent.IsSimple, true);
            Assert.Equals(textComponent.IsLiteral, false);
            Assert.Equals(textComponent.Text, "Root");
            Assert.That(textComponent.Extra, Is.EquivalentTo(["Extra1", "Extra2"]));
        });
    }

}