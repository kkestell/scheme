namespace Scheme.Tests.SpecialForms;

[TestFixture]
public class BeginTests
{
    [Test]
    public void TestBegin()
    {
        var env = new Env();

        var lexer = new Lexer(new InputBuffer("(begin (define x 10) (define y 20) (+ x y))"));
        var parser = new Parser(lexer);
        var beginObj = parser.Parse();

        var result = env.Eval(beginObj);

        Assert.That(result, Is.InstanceOf<NumberObj>());
        Assert.That(((NumberObj)result).Value, Is.EqualTo(30.0));
    }
}
