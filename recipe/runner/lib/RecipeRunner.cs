using CliWrap;
using Microsoft.Extensions.Logging;

namespace Tell;

public class RecipeRunner(ILogger<RecipeRunner> logger)
{
    public async Task<CommandResult> Run(Recipe recipe, string workingDirectory, Dictionary<string, string> variables)
    {
        logger.LogTrace("Building command from recipe: {Recipe}", recipe);

        var interpolated = recipe.InterpolateWith(variables);

        logger.LogDebug("Running in `{WorkingDirectory}` command: {Recipe}", workingDirectory, interpolated);

        var command = StandardCommand.From(interpolated, workingDirectory);
        return await command.ExecuteAsync();
    }

    public static Command BuildCommand(Recipe recipe, string workingDirectory, Dictionary<string, string> variables, bool pipeToConsole = true)
    {
        var interpolated = recipe.InterpolateWith(variables);
        return StandardCommand.From(interpolated, workingDirectory, pipeToConsole);
    }
}
