namespace Scheme.Tests;

[TestFixture]
public class ParserTests
{
    [Test]
    public void TestParseNumber()
    {
        var lexer = new Lexer(new InputBuffer("42"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        Assert.That(obj, Is.InstanceOf<NumberObj>());
        Assert.That(((NumberObj)obj).Value, Is.EqualTo(42));
    }

    [Test]
    public void TestParseIdentifier()
    {
        var lexer = new Lexer(new InputBuffer("x"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        Assert.That(obj, Is.InstanceOf<SymbolObj>());
        Assert.That(((SymbolObj)obj).Value, Is.EqualTo("x"));
    }

    [Test]
    public void TestParseString()
    {
        var lexer = new Lexer(new InputBuffer("\"hello\""));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        Assert.That(obj, Is.InstanceOf<StringObj>());
        Assert.That(((StringObj)obj).Value, Is.EqualTo("hello"));
    }

    [Test]
    public void TestParseBoolean()
    {
        var lexer = new Lexer(new InputBuffer("#t"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        Assert.That(obj, Is.InstanceOf<BooleanObj>());
        Assert.That(((BooleanObj)obj).Value, Is.EqualTo(true));
    }

    [Test]
    public void TestParseList()
    {
        var lexer = new Lexer(new InputBuffer("(1 2 x)"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        Assert.That(obj, Is.InstanceOf<ListObj>());
        var listObj = (ListObj)obj;

        var elements = listObj.Value;
        Assert.That(elements[0], Is.InstanceOf<NumberObj>());
        Assert.That(((NumberObj)elements[0]).Value, Is.EqualTo(1));

        Assert.That(elements[1], Is.InstanceOf<NumberObj>());
        Assert.That(((NumberObj)elements[1]).Value, Is.EqualTo(2));

        Assert.That(elements[2], Is.InstanceOf<SymbolObj>());
        Assert.That(((SymbolObj)elements[2]).Value, Is.EqualTo("x"));
    }

    [Test]
    public void TestParseEmptyList()
    {
        var lexer = new Lexer(new InputBuffer("()"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        Assert.That(obj, Is.InstanceOf<ListObj>());
        Assert.That(((ListObj)obj).Value, Is.EquivalentTo(new List<Obj>()));
    }
    
    [Test]
    public void TestParseSingleQuote()
    {
        var lexer = new Lexer(new InputBuffer("'x"));
        var parser = new Parser(lexer);
        var obj = parser.Parse();
        Assert.That(obj, Is.InstanceOf<ListObj>());

        var listObj = (ListObj)obj;
        Assert.That(listObj.Value.Count, Is.EqualTo(2));
    
        Assert.That(listObj.Value[0], Is.InstanceOf<SymbolObj>());
        Assert.That(((SymbolObj)listObj.Value[0]).Value, Is.EqualTo("quote"));

        Assert.That(listObj.Value[1], Is.InstanceOf<SymbolObj>());
        Assert.That(((SymbolObj)listObj.Value[1]).Value, Is.EqualTo("x"));
    }

    [Test]
    public void TestUnexpectedToken()
    {
        var lexer = new Lexer(new InputBuffer(")"));
        var parser = new Parser(lexer);
        Assert.Throws<ParsingException>(() => parser.Parse());
    }

    [Test]
    public void TestUnexpectedEof()
    {
        var lexer = new Lexer(new InputBuffer("("));
        var parser = new Parser(lexer);
        Assert.Throws<ParsingException>(() => parser.Parse());
    }
}