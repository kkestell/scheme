namespace Scheme
{
    public class Parser
    {
        private readonly Lexer _lexer;
        private Token _currentToken;
        private Token _nextToken;

        public Parser(Lexer lexer)
        {
            _lexer = lexer;
            _currentToken = _lexer.NextToken();
            _nextToken = _lexer.NextToken();
        }

        public Obj? Parse()
        {
            return ParseExpression();
        }

        private Obj? ParseExpression()
        {
            switch (_currentToken.Type)
            {
                case TokenType.Eof:
                    return null;
                case TokenType.Number:
                    return new NumberObj(double.Parse(ConsumeToken().Value));
                case TokenType.Identifier:
                    return new SymbolObj(ConsumeToken().Value);
                case TokenType.String:
                    return new StringObj(ConsumeToken().Value);
                case TokenType.Boolean:
                    return new BooleanObj(ConsumeToken().Value == "true");
                case TokenType.Quote:
                    ConsumeToken(TokenType.Quote);
                    return new ListObj(new List<Obj> { new SymbolObj("quote"), ParseExpression() });                
                case TokenType.LeftParen:
                    ConsumeToken(TokenType.LeftParen);
                    return ParseList();
                default:
                    throw new ParsingException($"Unexpected token: {_currentToken.Type}", _lexer.Location);
            }
        }

        private ListObj ParseList()
        {
            var elements = new List<Obj>();
            while (_currentToken.Type != TokenType.RightParen)
            {
                if (_currentToken.Type == TokenType.Eof)
                {
                    throw new ParsingException("Unexpected EOF while parsing list", _lexer.Location);
                }
                elements.Add(ParseExpression());
            }
            ConsumeToken(TokenType.RightParen);
            return new ListObj(elements);
        }

        private Token ConsumeToken()
        {
            var token = _currentToken;
            _currentToken = _nextToken;
            _nextToken = _lexer.NextToken();
            return token;
        }

        private Token ConsumeToken(TokenType expectedType)
        {
            if (_currentToken.Type == expectedType)
            {
                return ConsumeToken();
            }

            throw new ParsingException($"Unexpected token: Expected {expectedType}, got {_currentToken.Type}", _lexer.Location);
        }
    }
}
