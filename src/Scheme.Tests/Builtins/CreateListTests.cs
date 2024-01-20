namespace Scheme.Tests.Builtins;

[TestFixture]
public class CreateListTests
{
    [Test]
    public void TestList()
    {
        var env = new Env();
        var lexer = new Lexer(new InputBuffer("(list 1 2 3)"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        var result = env.Eval(obj);
        var resultList = ((ListObj)result).Value.Select(x => ((NumberObj)x).Value).ToList();
        Assert.That(resultList, Is.EqualTo(new List<double> { 1, 2, 3 }));
    }
}