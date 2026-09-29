using Copaster;

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

    public async Task Run(RuleRunParams parameters)
    {
        await Run(parameters.Rule.Recipes, parameters.WorkingDirectory.FullPath, parameters.Replacements);
    }
}

public record RuleRunParams(
    Rule Rule,
    Folder WorkingDirectory,
    IReadOnlyDictionary<string, string> Replacements
)
{
    public override string ToString() 
    {
        var replacements = string.Join(", ", Replacements.Select(kv => $"{kv.Key}={kv.Value}"));
        return $"Rule: {Rule.Name}, WorkingDirectory: {WorkingDirectory}, Replacements: {replacements}";
    }
}