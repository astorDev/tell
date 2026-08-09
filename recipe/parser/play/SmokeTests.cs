
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


        var parsed = Recipe.Parse(recipe);
        foreach (var element in parsed.Elements) Console.WriteLine(element);
    }
}