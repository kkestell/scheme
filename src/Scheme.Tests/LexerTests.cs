namespace Scheme.Tests;

[TestFixture]
public class LexerTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestParentheses()
    {
        var lexer = new Lexer(new InputBuffer("()"));
        var token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.LeftParen));
        Assert.That(token.Value, Is.EqualTo("("));

        token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.RightParen));
        Assert.That(token.Value, Is.EqualTo(")"));

        token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.Eof));
    }

    [Test]
    public void TestIdentifiers()
    {
        var lexer = new Lexer(new InputBuffer("abc def_ghi + - *"));
        var identifiers = new [] { "abc", "def_ghi", "+", "-", "*" };

        foreach (var identifier in identifiers)
        {
            var token = lexer.NextToken();
            Assert.That(token.Type, Is.EqualTo(TokenType.Identifier));
            Assert.That(token.Value, Is.EqualTo(identifier));
        }

        Assert.That(lexer.NextToken().Type, Is.EqualTo(TokenType.Eof));
    }

    [Test]
    public void TestNumbers()
    {
        var lexer = new Lexer(new InputBuffer("123 0.456"));
        var numbers = new [] { "123", "0.456" };

        foreach (var num in numbers)
        {
            var token = lexer.NextToken();
            Assert.That(token.Type, Is.EqualTo(TokenType.Number));
            Assert.That(token.Value, Is.EqualTo(num));
        }

        Assert.That(lexer.NextToken().Type, Is.EqualTo(TokenType.Eof));
    }

    [Test]
    public void TestStrings()
    {
        var lexer = new Lexer(new InputBuffer("\"hello\" \"world\""));
        var strings = new [] { "hello", "world" };

        foreach (var str in strings)
        {
            var token = lexer.NextToken();
            Assert.That(token.Type, Is.EqualTo(TokenType.String));
            Assert.That(token.Value, Is.EqualTo(str));
        }

        Assert.That(lexer.NextToken().Type, Is.EqualTo(TokenType.Eof));
    }

    [Test]
    public void TestBooleans()
    {
        var lexer = new Lexer(new InputBuffer("#t #f"));
        var bools = new [] { "true", "false" };

        foreach (var b in bools)
        {
            var token = lexer.NextToken();
            Assert.That(token.Type, Is.EqualTo(TokenType.Boolean));
            Assert.That(token.Value, Is.EqualTo(b));
        }

        Assert.That(lexer.NextToken().Type, Is.EqualTo(TokenType.Eof));
    }

    [Test]
    public void TestQuote()
    {
        var lexer = new Lexer(new InputBuffer("'a"));
        var token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.Quote));
        Assert.That(token.Value, Is.EqualTo("'"));

        token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(token.Value, Is.EqualTo("a"));

        Assert.That(lexer.NextToken().Type, Is.EqualTo(TokenType.Eof));
    }

    [Test]
    public void TestMixedTokens()
    {
        var lexer = new Lexer(new InputBuffer("(define x 10)"));
        var expectedTokens = new List<Token>
        {
            new() { Type = TokenType.LeftParen, Value = "(" },
            new() { Type = TokenType.Identifier, Value = "define" },
            new() { Type = TokenType.Identifier, Value = "x" },
            new() { Type = TokenType.Number, Value = "10" },
            new() { Type = TokenType.RightParen, Value = ")" },
            new() { Type = TokenType.Eof, Value = "EOF" }
        };

        foreach (var expected in expectedTokens)
        {
            var token = lexer.NextToken();
            Assert.That(token.Type, Is.EqualTo(expected.Type));
            Assert.That(token.Value, Is.EqualTo(expected.Value));
        }
    }
    
    [Test]
    public void TestInvalidBoolean()
    {
        var lexer = new Lexer(new InputBuffer("#x"));
        Assert.Throws<ParsingException>(() => lexer.NextToken());
    }

    [Test]
    public void TestUnterminatedString()
    {
        var lexer = new Lexer(new InputBuffer("\"hello"));
        Assert.Throws<ParsingException>(() => lexer.NextToken());
    }

    [Test]
    public void TestInvalidCharacter()
    {
        var lexer = new Lexer(new InputBuffer("$"));
        Assert.Throws<ParsingException>(() => lexer.NextToken());
    }

    [Test]
    public void TestInvalidNumber()
    {
        var lexer = new Lexer(new InputBuffer("123."));
        Assert.Throws<ParsingException>(() => lexer.NextToken());
    }

    [Test]
    public void TestInvalidNumberWithMultipleDots()
    {
        var lexer = new Lexer(new InputBuffer("123.45.67"));
        Assert.Throws<ParsingException>(() => lexer.NextToken());
    }
}