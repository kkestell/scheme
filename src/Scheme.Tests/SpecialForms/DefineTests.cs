namespace Scheme.Tests.SpecialForms;

[TestFixture]
public class DefineTests
{
    [Test]
    public void TestDefineSymbolToNumber()
    {
        var env = new Env();

        var lexer = new Lexer(new InputBuffer("(define x 10)"));
        var parser = new Parser(lexer);
        var defineObj = parser.Parse();
        env.Eval(defineObj);

        lexer = new Lexer(new InputBuffer("x"));
        parser = new Parser(lexer);
        var xObj = parser.Parse();
        var result = env.Eval(xObj);

        Assert.That(result, Is.InstanceOf<NumberObj>());
        Assert.That(((NumberObj)result).Value, Is.EqualTo(10));
    }

    [Test]
    public void TestDefineSymbolToLambda()
    {
        var env = new Env();

        var lexer = new Lexer(new InputBuffer("(define square (lambda (x) (* x x)))"));
        var parser = new Parser(lexer);
        var defineObj = parser.Parse();
        env.Eval(defineObj);

        lexer = new Lexer(new InputBuffer("(square 5)"));
        parser = new Parser(lexer);
        var squareObj = parser.Parse();
        var result = env.Eval(squareObj);

        Assert.That(result, Is.InstanceOf<NumberObj>());
        Assert.That(((NumberObj)result).Value, Is.EqualTo(25));
    }
}