namespace Scheme.Tests.Builtins;

[TestFixture]
public class FilterTests
{
    [Test]
    public void TestFilter()
    {
        var env = new Env();
        var lexer = new Lexer(new InputBuffer("(filter (lambda (x) (> x 2)) '(1 2 3 4 5))"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        var result = env.Eval(obj);
        var listResult = (ListObj)result;
        Assert.That(listResult.Value.Select(o => ((NumberObj)o).Value), Is.EquivalentTo(new[] { 3.0, 4.0, 5.0 }));
    }
}