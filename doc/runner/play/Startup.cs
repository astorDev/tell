using Microsoft.Extensions.Logging;
using Superpower;
using Tell;

namespace Playground;

public class Startup : Command
{
    private readonly Option<string> fileOption = new("--file")
    {
        Description = "The path to the Makefile to use.",
        Required = false
    };

    private readonly Argument<string> target = new("target")
    {
        Description = "The target to run",
        Arity = ArgumentArity.ZeroOrOne
    };

    private readonly ILogger<Startup> logger;

    public Startup(ILogger<Startup> logger) : base("startup", "Interpret the Makefile and provide startup results.")
    {
        Add(fileOption);
        Add(target);

        this.logger = logger;
    }

    public RuleRunParams Interpret(ParseResult parseResult)
    {
        var file = parseResult.GetValue(fileOption) ?? "Makefile";
        var workingDirectory = Directory.GetCurrentDirectory();
        var makefilePath = Path.Combine(workingDirectory, file);
        var targetName = parseResult.GetValue(target) ?? "default";

        logger.LogTrace("Searching Makefile with Path {makefilePath}", makefilePath);

        if (!File.Exists(makefilePath))
        {
            throw new FileNotFoundException($"Makefile not found at {makefilePath}");
        }

        var makefileContent = File.ReadAllText(makefilePath);

        var doc = Doc.Parser.Parse(makefileContent);
        var rule = targetName != null ? doc.GetRule(targetName) : doc.FirstRule;

        return new RuleRunParams(rule, workingDirectory, parseResult.UnmatchedTokens);
    }
}