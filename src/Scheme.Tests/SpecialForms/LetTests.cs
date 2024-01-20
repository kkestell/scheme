namespace Scheme.Tests.SpecialForms;

[TestFixture]
public class LetTests
{
    [Test]
    public void TestLet()
    {
        var env = new Env();

        var lexer = new Lexer(new InputBuffer("(let ((x 2) (y 3)) (* x y))"));
        var parser = new Parser(lexer);
        var letObj = parser.Parse();

        var result = env.Eval(letObj);

        Assert.That(result, Is.InstanceOf<NumberObj>());
        Assert.That(((NumberObj)result).Value, Is.EqualTo(6.0));
    }
}