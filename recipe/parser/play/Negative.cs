using Superpower;

namespace Tell.Playground;

[TestClass]
public class Negative
{
    [TestMethod]
    public void NewLineInTheMiddle()
    {
        var recipe =
"""
    dotnet run --environment $(ENV)
    something else
""";

        var parsed = Recipe.Parser.TryParse(recipe);

        if (parsed.HasValue)
        {
            foreach (var element in parsed.Value.Fragments) Console.WriteLine(element);
            parsed.Value.Fragments.Length.ShouldBe(2);
        }

        //parsed.HasValue.ShouldBeFalse();
    }
}