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
        
        return await startInfo.Run();
    }
}
