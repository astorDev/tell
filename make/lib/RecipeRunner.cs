using System.Diagnostics;
using Microsoft.Extensions.Logging;
using NiceShell;

namespace Tell;

public class RecipeRunner(ILogger<RecipeRunner> logger)
{
    public async Task<Process> Run(Recipe recipe, string workingDirectory, IReadOnlyDictionary<string, string> variables)
    {
        logger.LogTrace("Building command from recipe: {Recipe}", recipe);

        var interpolated = recipe.ToCommandString(variables);

        logger.LogDebug("Running built command in `{WorkingDirectory}`:", workingDirectory);
        logger.LogInformation("{Interpolated}", interpolated);

        var startInfo = Shell.Sh.ProxyProcessStartInfo(interpolated);
        startInfo.WorkingDirectory = workingDirectory;
        
        Console.Write(SelectGraphicRendition.Dim);
        var result = await startInfo.Run();
        Console.Write(SelectGraphicRendition.NormalIntensity);
        return result;
    }
}

public static class RecipeRunnerExtensions
{
    public static async Task RunAll(this RecipeRunner recipeRunner, IEnumerable<Recipe> recipes, string workingDirectory, IReadOnlyDictionary<string, string> variables)
    {
        foreach (var recipe in recipes)
        {
            using var process = await recipeRunner.Run(recipe, workingDirectory, variables);
            if (process.ExitCode != 0) throw new Exception($"Recipe failed with exit code {process.ExitCode}: {recipe}");
        }
    }
}