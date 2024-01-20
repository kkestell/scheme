namespace Scheme.Tests.Builtins;

[TestFixture]
public class ConsTests
{
    [Test]
    public void TestCons()
    {
        var env = new Env();
        var lexer = new Lexer(new InputBuffer("(cons 1 '(2 3))"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        var result = env.Eval(obj);
        var resultList = ((ListObj)result).Value.Select(x => ((NumberObj)x).Value).ToList();
        Assert.That(resultList, Is.EqualTo(new List<double> { 1, 2, 3 }));
    }
}