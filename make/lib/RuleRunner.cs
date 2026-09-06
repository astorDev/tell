namespace Tell;

public class RuleRunner(RecipeRunner recipeRunner)
{
    public async Task Run(IEnumerable<Recipe> recipes, string workingDirectory, IReadOnlyDictionary<string, string> variables)
    {
        foreach (var recipe in recipes)
        {
            using var process = await recipeRunner.Run(recipe, workingDirectory, variables);
            if (process.ExitCode != 0) throw new Exception($"Recipe failed with exit code {process.ExitCode}: {recipe}");
        }
    }
}