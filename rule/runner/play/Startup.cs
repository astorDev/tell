using Microsoft.Extensions.Logging;
using Superpower;
using Tell;

namespace Playground;

public record RunParams(
    Rule Rule,
    string WorkingDirectory,
    IReadOnlyList<string> RemainingArgs
);

public class Startup : Command
{
    private readonly Option<string> fileOption = new("--file")
    {
        Description = "The path to the Makefile to use.",
        Required = false
    };

    private readonly ILogger<Startup> logger;

    public Startup(ILogger<Startup> logger) : base("startup", "Interpret the Makefile and provide startup results.")
    {
        Add(fileOption);

        this.logger = logger;
    }

    public RunParams Interpret(ParseResult parseResult)
    {
        var file = parseResult.GetValue(fileOption) ?? "Makefile";
        var workingDirectory = Directory.GetCurrentDirectory();
        var makefilePath = Path.Combine(workingDirectory, file);

        logger.LogTrace("Searching Makefile with Path {makefilePath}", makefilePath);

        if (!File.Exists(makefilePath))
        {
            throw new FileNotFoundException($"Makefile not found at {makefilePath}");
        }

        var makefileContent = File.ReadAllText(makefilePath);

        var rule = Rule.Parser.Parse(makefileContent);
        return new RunParams(rule, workingDirectory, parseResult.UnmatchedTokens);
    }
}