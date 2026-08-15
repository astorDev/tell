using CliWrap;
using Microsoft.Extensions.Logging;

namespace Tell;

public class RecipeRunner(ILogger<RecipeRunner> logger)
{
    public async Task<CommandResult> Run(Recipe recipe, string workingDirectory, IReadOnlyDictionary<string, string> variables)
    {
        logger.LogTrace("Building command from recipe: {Recipe}", recipe);

        var interpolated = recipe.ToCommandString(variables);

        logger.LogDebug("Running built command in `{WorkingDirectory}`:", workingDirectory);
        logger.LogInformation("{Interpolated}", interpolated);

        var command = StandardCommand.From(interpolated, workingDirectory);
        return await command.ExecuteAsync();
    }

    public static Command BuildCommand(Recipe recipe, string workingDirectory, IReadOnlyDictionary<string, string> variables, bool pipeToConsole = true)
    {
        var interpolated = recipe.ToCommandString(variables);
        return StandardCommand.From(interpolated, workingDirectory, pipeToConsole);
    }
}
