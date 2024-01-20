namespace Scheme.Tests.Builtins;

[TestFixture]
public class DisplayTests
{
    [Test]
    public void TestDisplay()
    {
        using StringWriter sw = new StringWriter();
        Console.SetOut(sw);
        
        var env = new Env();
        var lexer = new Lexer(new InputBuffer("(display \"hello\")"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        var result = env.Eval(obj);

        Assert.That(sw.ToString(), Is.EqualTo("hello"));
        Assert.That(result, Is.InstanceOf<SymbolObj>());
        Assert.That(((SymbolObj)result).Value, Is.EqualTo("ok"));
    }
}