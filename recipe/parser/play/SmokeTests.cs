using Superpower;

namespace Tell.Playground;

[TestClass]
public class SmokeTests
{
    [TestMethod]
    public void Basic()
    {
        var recipe = 
"""
    dotnet run --environment $(ENV)
""";

        var parsed = Recipe.Parser.Parse(recipe);
        foreach (var element in parsed.Fragments) Console.WriteLine(element);
    }

    [TestMethod]
    public void WithNewLineInTheEnd()
    {
        var recipe =
"""
    dotnet run --environment $(ENV)

""";

        var parsed = Recipe.Parser.Parse(recipe);
        Console.WriteLine(parsed);
        foreach (var element in parsed.Fragments) Console.WriteLine(element);
    }
}