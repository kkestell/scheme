namespace Scheme.Tests.SpecialForms;

[TestFixture]
public class LambdaTests
{
    private Env _env;

    [SetUp]
    public void Setup()
    {
        _env = new Env();
    }

    [Test]
    public void TestLambdaCreatesClosure()
    {
        var lexer = new Lexer(new InputBuffer("(define square (lambda (x) (* x x)))"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        _env.Eval(obj);
        Assert.That(_env.Lookup("square").GetType(), Is.EqualTo(typeof(ClosureObj)));
    }

    [Test]
    public void TestLambdaApplication()
    {
        var defineLexer = new Lexer(new InputBuffer("(define square (lambda (x) (* x x)))"));
        var defineParser = new Parser(defineLexer);
        var defineObj = defineParser.Parse();
        _env.Eval(defineObj);

        var callLexer = new Lexer(new InputBuffer("(square 4)"));
        var callParser = new Parser(callLexer);
        var callObj = callParser.Parse();
        var result = _env.Eval(callObj);
        
        Assert.That(result.GetType(), Is.EqualTo(typeof(NumberObj)));
        Assert.That(((NumberObj)result).Value, Is.EqualTo(16.0));
    }

    [Test]
    public void TestLambdaWithMultipleArgs()
    {
        var lexer1 = new Lexer(new InputBuffer("(define add (lambda (x y) (+ x y)))"));
        var parser1 = new Parser(lexer1);
        var obj1 = parser1.Parse();
        _env.Eval(obj1);

        var lexer2 = new Lexer(new InputBuffer("(add 3 4)"));
        var parser2 = new Parser(lexer2);
        var obj2 = parser2.Parse();
        var result = _env.Eval(obj2);

        Assert.That(result.GetType(), Is.EqualTo(typeof(NumberObj)));
        Assert.That(((NumberObj)result).Value, Is.EqualTo(7.0));
    }

    [Test]
    public void TestLambdaNoArgs()
    {
        var lexer1 = new Lexer(new InputBuffer("(define const-five (lambda () 5))"));
        var parser1 = new Parser(lexer1);
        var obj1 = parser1.Parse();
        _env.Eval(obj1);

        var lexer2 = new Lexer(new InputBuffer("(const-five)"));
        var parser2 = new Parser(lexer2);
        var obj2 = parser2.Parse();
        var result = _env.Eval(obj2);

        Assert.That(result.GetType(), Is.EqualTo(typeof(NumberObj)));
        Assert.That(((NumberObj)result).Value, Is.EqualTo(5.0));
    }
}