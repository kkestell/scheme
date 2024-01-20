namespace Scheme.Tests.Builtins;

[TestFixture]
public class GreaterThanOrEqualTests
{
    private Env _env;

    [SetUp]
    public void Setup()
    {
        _env = new Env();
    }

    [Test]
    public void TestGreaterThanOrEqual()
    {
        var lexer = new Lexer(new InputBuffer("(>= 3 2)"));
        var parser = new Parser(lexer);
        var result = _env.Eval(parser.Parse());
        Assert.That(((BooleanObj)result).Value, Is.True);
    }
}