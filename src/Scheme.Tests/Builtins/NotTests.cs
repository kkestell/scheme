namespace Scheme.Tests.Builtins;

[TestFixture]
public class NotTests
{
    private Env _env;

    [SetUp]
    public void Setup()
    {
        _env = new Env();
    }

    [Test]
    public void TestNot()
    {
        var lexer = new Lexer(new InputBuffer("(not #t)"));
        var parser = new Parser(lexer);
        var result = _env.Eval(parser.Parse());
        Assert.That(((BooleanObj)result).Value, Is.False);
    }
}