namespace Scheme.Tests.Builtins;

[TestFixture]
public class MapTests
{
    [Test]
    public void TestMap()
    {
        var env = new Env();
        var lexer = new Lexer(new InputBuffer("(map (lambda (x) (* x x)) '(1 2 3))"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        var result = env.Eval(obj);

        var expectedList = new ListObj(new List<Obj> 
        {
            new NumberObj(1),
            new NumberObj(4),
            new NumberObj(9)
        });

        Assert.That(result, Is.EqualTo(expectedList));
    }
}