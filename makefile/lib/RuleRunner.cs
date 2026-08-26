namespace Tell;

public class RuleRunner(RecipeRunner recipeRunner)
{
    public async Task Run(IEnumerable<Recipe> recipes, string workingDirectory, IReadOnlyDictionary<string, string> variables)
    {
        foreach (var recipe in recipes)
        {
            await recipeRunner.Run(recipe, workingDirectory, variables);
        }
    }
}