namespace Scheme.Tests.Builtins;

[TestFixture]
public class LessThanOrEqualTests
{
    private Env _env;

    [SetUp]
    public void Setup()
    {
        _env = new Env();
    }

    [Test]
    public void TestLessThanOrEqual()
    {
        var lexer = new Lexer(new InputBuffer("(<= 3 3)"));
        var parser = new Parser(lexer);
        var result = _env.Eval(parser.Parse());
        Assert.That(((BooleanObj)result).Value, Is.True);
    }
}