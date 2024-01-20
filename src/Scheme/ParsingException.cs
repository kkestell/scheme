namespace Scheme;

public class ParsingException : Exception
{
    private readonly Location _location;
    
    public ParsingException(string message, Location location) : base(message)
    {
        _location = location;
    }
}