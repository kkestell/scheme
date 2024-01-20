namespace Scheme.Tests.Builtins;

[TestFixture]
public class NewlineTests
{
    [Test]
    public void TestNewline()
    {
        using StringWriter sw = new StringWriter();
        Console.SetOut(sw);
        
        var env = new Env();
        var lexer = new Lexer(new InputBuffer("(newline)"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        var result = env.Eval(obj);

        Assert.That(sw.ToString(), Is.EqualTo(Environment.NewLine));
        Assert.That(result, Is.InstanceOf<SymbolObj>());
        Assert.That(((SymbolObj)result).Value, Is.EqualTo("ok"));
    }
}