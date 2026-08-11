using Tell;

namespace Playground;

[TestClass]
public class SmokeTests
{
    [TestMethod]
    public void Basic()
    {
        var example =
"""
ENV ?= dev

run:
    dotnet run --environment $(ENV)

test:
    dotnet test --filter FullyQualifiedName~Tell.Playground.LineSplitTests --logger "console;verbosity=detailed"
""";

        var tokenizer = NewLine.SplitTokenizer<string>(b => b.MatchAnything());

        var tokens = tokenizer.Tokenize(example);
        
        tokens.ForEach(t => Console.WriteLine(t));
    }
}