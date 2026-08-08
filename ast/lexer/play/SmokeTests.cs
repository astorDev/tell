namespace Tell;

[TestClass]
public class SmokeTests
{
    [TestMethod]
    public void Main()
    {
        var makefile = 
"""
TEST ?= main

run:
    dotnet run

kill:
    kill lsof -t -i:52873
""";

        var tokens = MakefileLexer.Tokenizer.Tokenize(makefile).ToArray();

        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
    }
}