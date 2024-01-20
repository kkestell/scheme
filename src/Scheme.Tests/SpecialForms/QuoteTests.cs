namespace Scheme.Tests.SpecialForms;

[TestFixture]
public class QuoteTests
{
    [Test]
    public void TestQuote()
    {
        var env = new Env();

        var lexer = new Lexer(new InputBuffer("(quote (1 2 3))"));
        var parser = new Parser(lexer);
        var quoteObj = parser.Parse();

        var result = env.Eval(quoteObj);

        Assert.That(result, Is.InstanceOf<ListObj>());
        var resultList = (ListObj)result;
        Assert.That(resultList.Value.Count, Is.EqualTo(3));
        Assert.That(resultList.Value[0], Is.InstanceOf<NumberObj>());
        Assert.That(resultList.Value[1], Is.InstanceOf<NumberObj>());
        Assert.That(resultList.Value[2], Is.InstanceOf<NumberObj>());
    }
}