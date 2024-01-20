namespace Scheme.Tests.SpecialForms;

[TestFixture]
public class WhileTests
{
    [Test]
    public void TestWhile()
    {
        var env = new Env();
        env.Define("x", new NumberObj(0));
        var lexer = new Lexer(new InputBuffer("(while (< x 5) (define x (+ x 1)))"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        var result = env.Eval(obj);
        Assert.That(result, Is.InstanceOf<SymbolObj>());
        Assert.That(((SymbolObj)result).Value, Is.EqualTo("ok"));
        Assert.That(((NumberObj)env.Lookup("x")).Value, Is.EqualTo(5));
    }
}
