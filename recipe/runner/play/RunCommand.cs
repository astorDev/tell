using Microsoft.Extensions.Logging;
using Superpower;

namespace Tell;

public class RunCommand : Command
{
    private readonly ILogger<RunCommand> logger;
    private readonly RecipeRunner runner;

    public RunCommand(ILogger<RunCommand> logger, RecipeRunner runner) : base("run", "Run hard-coded recipe with hard-coded arguments")
    {
        this.logger = logger;
        this.runner = runner;

        SetAction(Execute);
    }

    private async Task Execute(ParseResult parseResult)
    {
        var recipe = Recipe.Parser.Parse(
"""
    echo "Hello, $(NAME)!"
""");

        var variables = new Dictionary<string, string> { { "NAME", "Tell" } };
        var workingDirectory = Directory.GetCurrentDirectory();

        var result = await runner.Run(recipe, workingDirectory, variables);

        logger.LogInformation("Recipe executed with exit code: {ExitCode}", result.ExitCode);
    }
}