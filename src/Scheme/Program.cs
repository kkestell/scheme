namespace Scheme;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 1)
        {
            var fileContent = File.ReadAllText(args[0]);
            var env = new Env();
            var lexer = new Lexer(new InputBuffer(fileContent));
            var parser = new Parser(lexer);
            while (true)
            {
                var obj = parser.Parse();
                if (obj == null)
                {
                    break;
                }
                env.Eval(obj);
            }
        }
        else
        {
            var env = new Env();
            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                if (line == "exit")
                {
                    break;
                }
                try
                {
                    var lexer = new Lexer(new InputBuffer(line));
                    var parser = new Parser(lexer);
                    var obj = parser.Parse();
                    var result = env.Eval(obj);
                    Console.WriteLine(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }
            }
        }
    }
}