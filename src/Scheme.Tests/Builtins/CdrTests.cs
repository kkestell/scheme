namespace Scheme.Tests.Builtins;

[TestFixture]
public class CdrTests
{
    [Test]
    public void TestCdr()
    {
        var env = new Env();
        var lexer = new Lexer(new InputBuffer("(cdr '(1 2 3))"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        var result = env.Eval(obj);
        Assert.That(result, Is.InstanceOf<ListObj>());
        var resultList = ((ListObj)result).Value;
    
        Assert.That(resultList.Count, Is.EqualTo(2));
        Assert.That(((NumberObj)resultList[0]).Value, Is.EqualTo(2));
        Assert.That(((NumberObj)resultList[1]).Value, Is.EqualTo(3));
    }
}