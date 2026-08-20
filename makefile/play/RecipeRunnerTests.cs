using Superpower;

namespace Tell.Playground;

[TestClass]
public class RecipeRunnerTests
{
    [TestMethod]
    public void Basic()
    {
        var recipeString = 
"""
    dotnet run --environment $(ENV)
""";

        var recipe = Recipe.Parser.Parse(recipeString);
        var interpolated = recipe.ToCommandString(new Dictionary<string, string> { ["ENV"] = "Development" });

        Console.WriteLine(interpolated);
    }
}