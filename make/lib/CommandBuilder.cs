namespace Tell;

public static class RecipeCommands
{
    public static IEnumerable<string> From(Makefile makefile, Rule rule, IReadOnlyDictionary<string, string> variables)
    {
        foreach (var ruleToRun in RuleDependencyTraversal.Ordered(makefile, rule))
        {
            foreach (var recipe in ruleToRun.Recipes)
            {
                yield return recipe.ToCommandString(variables);
            }
        }
    }
}