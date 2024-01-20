namespace Scheme.Tests.Builtins;

[TestFixture]
public class IsNullTests
{
    [Test]
    public void TestIsNull()
    {
        var env = new Env();
        var lexer = new Lexer(new InputBuffer("(null? '())"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        var result = env.Eval(obj);
        Assert.IsTrue(((BooleanObj)result).Value);
    }
}