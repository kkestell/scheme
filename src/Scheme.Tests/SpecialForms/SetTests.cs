namespace Scheme.Tests.SpecialForms;

[TestFixture]
public class SetTests
{
    [Test]
    public void TestSet()
    {
        var env = new Env();
        env.Define("x", new NumberObj(0));
        var lexer = new Lexer(new InputBuffer("(set! x 1)"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        var result = env.Eval(obj);
        Assert.That(result, Is.InstanceOf<SymbolObj>());
        Assert.That(((SymbolObj)result).Value, Is.EqualTo("ok"));
        Assert.That(((NumberObj)env.Lookup("x")).Value, Is.EqualTo(1));
    }
}