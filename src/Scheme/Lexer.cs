using System.Text;

namespace Scheme;

public class Lexer
{
    private readonly InputBuffer _input;
    
    public Lexer(InputBuffer input)
    {
        _input = input;
    }
    
    public Location Location => _input.Location;
    
    public Token NextToken()
    {
        while (!_input.IsEmpty())
        {
            var ch = _input.Peek();
            if (char.IsWhiteSpace(ch))
            {
                _input.Pop();
                continue;
            }

            switch (ch)
            {
                case '(':
                    _input.Pop();
                    return new Token { Type = TokenType.LeftParen, Value = "(" };
                case ')':
                    _input.Pop();
                    return new Token { Type = TokenType.RightParen, Value = ")" };
                case '"':
                {
                    var sb = new StringBuilder();
                    _input.Pop();

                    while (true)
                    {
                        if (_input.IsEmpty())
                        {
                            throw new ParsingException("Unterminated string", _input.Location);
                        }
                        if (_input.Peek() == '"')
                        {
                            break;
                        }
                        sb.Append(_input.Peek());
                        _input.Pop();
                    }

                    _input.Pop();
                    return new Token { Type = TokenType.String, Value = sb.ToString() };
                }
            }

            if (char.IsDigit(ch) || (ch == '-' && char.IsDigit(_input.Peek(1))))
            {
                var sb = new StringBuilder();
                bool dotSeen = false;
        
                while (true)
                {
                    var nextChar = _input.Peek();

                    if (nextChar == '.')
                    {
                        if (dotSeen)
                        {
                            throw new ParsingException("Invalid number token", _input.Location);
                        }
                        dotSeen = true;
                    }
                    else if (!char.IsDigit(nextChar))
                    {
                        if (dotSeen && sb[^1] == '.')
                        {
                            throw new ParsingException("Invalid number token", _input.Location);
                        }
                        break;
                    }
            
                    sb.Append(nextChar);
                    _input.Pop();
                }
        
                return new Token { Type = TokenType.Number, Value = sb.ToString() };
            }

            if (ch == '#')
            {
                _input.Pop();
                var boolChar = _input.Peek();
                _input.Pop();
                
                switch (boolChar)
                {
                    case 't':
                    case 'T':
                        return new Token { Type = TokenType.Boolean, Value = "true" };
                    case 'f':
                    case 'F':
                        return new Token { Type = TokenType.Boolean, Value = "false" };
                    default:
                        throw new ParsingException("Invalid boolean token", _input.Location);
                }
            }

            if (char.IsLetter(ch) || "_+-*/<=>!?&%".Contains(ch))
            {
                var sb = new StringBuilder();
                
                while (char.IsLetterOrDigit(_input.Peek()) || "_+-*/<=>!?&%".Contains(_input.Peek()))
                {
                    sb.Append(_input.Peek());
                    _input.Pop();
                }
                
                return new Token { Type = TokenType.Identifier, Value = sb.ToString() };
            }

            if (ch == '\'')
            {
                _input.Pop();
                
                return new Token { Type = TokenType.Quote, Value = "'" };
            }

            throw new ParsingException($"Invalid character {ch}", _input.Location);
        }

        return new Token { Type = TokenType.Eof, Value = "EOF" };
    }
}