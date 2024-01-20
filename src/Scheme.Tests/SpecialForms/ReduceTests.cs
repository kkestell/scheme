namespace Scheme.Tests.SpecialForms;

[TestFixture]
public class ReduceTests
{
    [Test]
    public void TestReduce()
    {
        var env = new Env();
            
        var lexer = new Lexer(new InputBuffer("(reduce + '(1 2 3 4) 0)"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        var result = env.Eval(obj);
        Assert.That(result, Is.InstanceOf<NumberObj>());
        Assert.That(((NumberObj)result).Value, Is.EqualTo(10));
    }
}
