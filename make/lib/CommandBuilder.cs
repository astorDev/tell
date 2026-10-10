namespace Tell;

public static class RecipeCommands
{
    public static IEnumerable<string> From(Rule rule, IReadOnlyDictionary<string, string> variables)
    {
        foreach (var recipe in rule.RecipesFromDependenciesAndBody)
        {
            yield return recipe.ToCommandString(variables);
        }
    }
}