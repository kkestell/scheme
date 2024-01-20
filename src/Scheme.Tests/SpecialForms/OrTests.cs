namespace Scheme.Tests.SpecialForms;

[TestFixture]
public class OrTests
{
    [Test]
    public void TestOr()
    {
        var env = new Env();
        var lexer = new Lexer(new InputBuffer("(or #t #f)"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        var result = env.Eval(obj);
        Assert.That(result, Is.InstanceOf<BooleanObj>());
        Assert.That(((BooleanObj)result).Value, Is.True);
    }
}