namespace Tell;

public class RuleRunner(RecipeRunner recipeRunner)
{
    public async Task Run(Rule rule, string workingDirectory, IReadOnlyDictionary<string, string> variables)
    {
        foreach (var recipe in rule.Recipes)
        {
            await recipeRunner.Run(recipe, workingDirectory, variables);
        }
    }
}