using CliWrap;
using Microsoft.Extensions.Logging;
using NiceShell;

namespace Tell;

public class RecipeRunner(ILogger<RecipeRunner> logger)
{
    public async Task<CommandResult> Run(Recipe recipe, string workingDirectory, IReadOnlyDictionary<string, string> variables)
    {
        logger.LogTrace("Building command from recipe: {Recipe}", recipe);

        var interpolated = recipe.ToCommandString(variables);

        logger.LogDebug("Running built command in `{WorkingDirectory}`:", workingDirectory);
        logger.LogInformation("{Interpolated}", interpolated);

        var shell = OperatingSystem.IsWindows() ? Shell.Cmd : Shell.Bash;
        var command = shell.Proxy(interpolated)
            .WithWorkingDirectory(workingDirectory)
            .WithConsoleForwarding();

        return await command.ExecuteAsync();
    }
}
