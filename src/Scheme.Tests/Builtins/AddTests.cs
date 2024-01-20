namespace Scheme.Tests.Builtins;

[TestFixture]
public class AddTests
{
    [Test]
    public void TestAdd()
    {
        var env = new Env();

        var lexer = new Lexer(new InputBuffer("(+ 1 2 3)"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();

        var result = env.Eval(obj);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<NumberObj>());
        Assert.That(((NumberObj)result).Value, Is.EqualTo(6));
    }
}