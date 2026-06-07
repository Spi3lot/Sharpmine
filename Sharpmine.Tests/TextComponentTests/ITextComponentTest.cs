namespace Sharpmine.Tests.TextComponentTests;

public interface ITextComponentTest
{

    public const string SerializedLiteral = "\"Hello!\"";

    public const string SerializedList = """["Root","Extra1","Extra2"]""";

    public const string SerializedComplexAsObject = """{"text":"Complex","bold":true,"italic":true,"extra":["Literal", {"text":"Complex","bold":true,"italic":true}]}""";
    
    public const string SerializedComplexAsList = """[{"text":"Complex","bold":true,"italic":true},"Literal",{"text":"Complex","bold":true,"italic":true}]""";

    public Task Empty();

    public Task Literal();

    public Task List();

    public Task Complex();

    public Task ShadowColor();

}