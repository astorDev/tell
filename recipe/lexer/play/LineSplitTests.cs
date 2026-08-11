using Superpower;
using Superpower.Model;

namespace Tell.Playground;

[TestClass]
public class LineSplitTests
{
    public const string Example =
"""
ENV ?= dev

run:
    dotnet run --environment $(ENV)

test:
    dotnet test --filter FullyQualifiedName~Tell.Playground.LineSplitTests --logger "console;verbosity=detailed"
""";

    private static readonly Tokenizer<string> LineTokenizer = NewLine.SplitTokenizer<string>(b => b.MatchRecipes().MatchAnything());

    public static TokenList<string> ExampleLineTokens => LineTokenizer.Tokenize(Example);

    [TestMethod]
    public void RecipeLines() => ExampleLineTokens.ForEach(t => Console.WriteLine(t));

    [TestMethod]
    public void InsideLine()
    {
        var insideTokenizer = Anything.Tokenizer(b => b.MatchTabs());

        foreach (var lineToken in ExampleLineTokens.Where(t => t.Kind == RecipeLine.Key))
        {
            var insideTokens = insideTokenizer.Tokenize(lineToken.ToStringValue());
            insideTokens.ForEach(t => Console.WriteLine(t));
        }
    }
}