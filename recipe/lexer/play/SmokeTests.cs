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

        var tokens = RecipeTokens.Tokenizer.Tokenize(recipe);
        
        foreach (var token in tokens) Console.WriteLine(token);
    }
}