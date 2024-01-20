namespace Scheme.Tests;

[TestFixture]
public class EnvTests
{
    [Test]
    public void TestDefine()
    {
      var env = new Env();
      var lexer = new Lexer(new InputBuffer("(define x 10)"));
      var parser = new Parser(lexer);
      var obj = parser.Parse();
      env.Eval(obj);
      Assert.That(((NumberObj)env.Lookup("x")).Value, Is.EqualTo(10.0));
    }

    [Test]
    public void TestSet()
    {
      var env = new Env();
      var lexer = new Lexer(new InputBuffer("(define x 10)"));
      var parser = new Parser(lexer);
      var obj = parser.Parse();
      env.Eval(obj);

      lexer = new Lexer(new InputBuffer("(set! x 20)"));
      parser = new Parser(lexer);
      obj = parser.Parse();
      env.Eval(obj);

      Assert.That(((NumberObj)env.Lookup("x")).Value, Is.EqualTo(20.0));
    }

    [Test]
    public void TestLookupUndefinedSymbolReturnsNull()
    {
      var env = new Env();
      Assert.That(env.Lookup("undefined_symbol"), Is.Null);
    }
    
    [Test]
    public void TestClosureCapturesEnvironment()
    {
        var env = new Env();

        var lexer = new Lexer(new InputBuffer("(define x 42)"));
        var parser = new Parser(lexer);
        var defineObj = parser.Parse();
        env.Eval(defineObj);

        lexer = new Lexer(new InputBuffer("((lambda (y) (+ x y)) 8)"));
        parser = new Parser(lexer);
        var adderObj = parser.Parse();
        
        var result = env.Eval(adderObj);
        
        Assert.That(((NumberObj)result).Value, Is.EqualTo(50.0));
    }
}