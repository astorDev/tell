namespace Tell.Playground;

[TestClass]
public class SmokeTests
{
    [TestMethod]
    public void Basic()
    {
        var recipeString = 
"""

    dotnet run --environment $(ENV)
""";

        var recipe = Recipe.Parse(recipeString);
        var interpolated = recipe.InterpolateWith(new Dictionary<string, string> { ["ENV"] = "Development" });

        Console.WriteLine(interpolated);
    }
}