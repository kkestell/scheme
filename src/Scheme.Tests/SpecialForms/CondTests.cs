namespace Scheme.Tests.SpecialForms;

[TestFixture]
public class CondTests
{
    [Test]
    public void TestCond()
    {
        var env = new Env();
        var lexer = new Lexer(new InputBuffer("(cond (#f 1) (#t 2) (#t 3))"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        var result = env.Eval(obj);
        Assert.That(result, Is.InstanceOf<NumberObj>());
        Assert.That(((NumberObj)result).Value, Is.EqualTo(2));
    }
}