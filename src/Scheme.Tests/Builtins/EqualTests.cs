namespace Scheme.Tests.Builtins;

[TestFixture]
public class EqualTests
{
    private Env _env;

    [SetUp]
    public void Setup()
    {
        _env = new Env();
    }

    [Test]
    public void TestEqual()
    {
        var lexer = new Lexer(new InputBuffer("(= 3 3)"));
        var parser = new Parser(lexer);
        var result = _env.Eval(parser.Parse());
        Assert.That(((BooleanObj)result).Value, Is.True);
    }
}