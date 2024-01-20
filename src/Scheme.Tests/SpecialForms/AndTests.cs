namespace Scheme.Tests.SpecialForms;

[TestFixture]
public class AndTests
{
    [Test]
    public void TestAnd()
    {
        var env = new Env();
        var lexer = new Lexer(new InputBuffer("(and #t #f)"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        var result = env.Eval(obj);
        Assert.That(result, Is.InstanceOf<BooleanObj>());
        Assert.That(((BooleanObj)result).Value, Is.False);
    }
}