namespace Tell.Playground;

[TestClass]
public class RecipeTests
{
    [TestMethod] public void Basic() => Check("dotnet run --environment $(ENV)", new () { { "ENV", "dev" } });
    [TestMethod] public void WithNewLineInTheEnd() => Check("dotnet run --environment $(ENV)\n", new () { { "ENV", "dev" } });
    [TestMethod] public void WithVariableEscaped() => Check("echo $$PATH", null);
    [TestMethod] public void WithFunctionCallEscaped() => Check("echo $$(date)", null);

    public void Check(string recipeContent, Dictionary<string, string>? variables = null)
    {
        var recipeString = "\t" + recipeContent;
        variables ??= [];
        var parsed = Recipe.Parser.Parse(recipeString);
        Console.WriteLine(parsed);
        Console.WriteLine(parsed.ToCommandString(variables));
    }
}