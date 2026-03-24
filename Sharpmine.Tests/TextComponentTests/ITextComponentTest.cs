namespace Sharpmine.Tests.TextComponentTests;

public interface ITextComponentTest
{

    public const string SerializedLiteral = "\"Hello!\"";

    public const string SerializedList = """["Root","Extra1","Extra2"]""";

    public const string SerializedComplexAsObject = """{"type":"text","text":"Complex","bold":true,"italic":true,"extra":["Literal", {"type":"text","text":"Complex","bold":true,"italic":true}]}""";
    
    public const string SerializedComplexAsList = """[{"type":"text","text":"Complex","bold":true,"italic":true},"Literal",{"type":"text","text":"Complex","bold":true,"italic":true}]""";

    public Task Empty();

    public Task Literal();

    public Task List();

    public Task Complex();

    public Task ShadowColor();

}