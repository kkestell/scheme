namespace Scheme.Tests.SpecialForms;

[TestFixture]
public class IfTests
{
    [Test]
    public void TestIf()
    {
        var env = new Env();
        
        var lexer = new Lexer(new InputBuffer("(if (= 1 1) 42 13)"));
        var parser = new Parser(lexer);
        var ifObj = parser.Parse();
            
        var result = env.Eval(ifObj);
        Assert.That(((NumberObj)result).Value, Is.EqualTo(42.0));

        lexer = new Lexer(new InputBuffer("(if (= 1 0) 42 13)"));
        parser = new Parser(lexer);
        ifObj = parser.Parse();
            
        result = env.Eval(ifObj);
        Assert.That(((NumberObj)result).Value, Is.EqualTo(13.0));

        lexer = new Lexer(new InputBuffer("(if (= 1 0) 42)"));
        parser = new Parser(lexer);
        ifObj = parser.Parse();
            
        result = env.Eval(ifObj);
        Assert.That(((SymbolObj)result).Value, Is.EqualTo("unspecified"));
    }
}