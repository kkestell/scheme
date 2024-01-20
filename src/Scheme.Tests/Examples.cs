namespace Scheme.Tests;

[TestFixture]
public class Examples
{
    [Test]
    public void FactorialTest()
    {
        var env = new Env();
        
        // Redirect standard output
        var output = new StringWriter();
        Console.SetOut(output);

        // Read from file
        var filePath = "/home/kyle/src/scheme/examples/factorial.scm";
        var source = File.ReadAllText(filePath);

        var lexer = new Lexer(new InputBuffer(source));
        var parser = new Parser(lexer);
        do
        {
            var obj = parser.Parse();
            if (obj is null)
                break;
            env.Eval(obj);
        } while (true);
        
        // Capture and validate the output
        var capturedOutput = output.ToString();
        var expectedOutput = @"Factorial of 1 is 1
Factorial of 2 is 2
Factorial of 3 is 6
Factorial of 4 is 24
Factorial of 5 is 120
Factorial of 6 is 720
Factorial of 7 is 5040
Factorial of 8 is 40320
Factorial of 9 is 362880
Factorial of 10 is 3628800
";
        Assert.That(capturedOutput, Is.EqualTo(expectedOutput));
    }
}