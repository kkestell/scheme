namespace Scheme.Tests.Builtins;

[TestFixture]
public class LessThanTests
{
    private Env _env;

    [SetUp]
    public void Setup()
    {
        _env = new Env();
    }

    [Test]
    public void TestLessThan()
    {
        var lexer = new Lexer(new InputBuffer("(< 2 3)"));
        var parser = new Parser(lexer);
        var result = _env.Eval(parser.Parse());
        Assert.That(((BooleanObj)result).Value, Is.True);
    }
}