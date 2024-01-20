namespace Scheme.Tests.Builtins;

[TestFixture]
public class CarTests
{
    [Test]
    public void TestCar()
    {
        var env = new Env();
        var lexer = new Lexer(new InputBuffer("(car '(1 2 3))"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        var result = env.Eval(obj);
        Assert.That(((NumberObj)result).Value, Is.EqualTo(1));
    }
}