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
        var commandsToRun = RecipeCommands.From(parameters.Rule, parameters.Replacements);
        foreach (var command in commandsToRun)
        {
            using var process = await recipeRunner.Run(command, parameters.WorkingDirectory.FullPath);
            if (process.ExitCode != 0) throw new Exception($"Command failed with exit code {process.ExitCode}: {command}");
        }
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