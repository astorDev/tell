using System.CommandLine;
using Microsoft.Extensions.Logging;
using Superpower;

namespace Tell;

public class TellCommand : Command
{
    private static readonly Argument<string> firstArgument = new("first")
    {
        Description = "The first positional argument. Can be: working directory, target or first argument of the rule.",
        Arity = ArgumentArity.ZeroOrOne
    };

    private static readonly Argument<string> secondArgument = new("second")
    {
        Description = "The second positional argument. Can be: target or first argument of the rule.",
        Arity = ArgumentArity.ZeroOrOne
    };

    private static readonly Argument<string> thirdArgument = new("third")
    {
        Description = "The third positional argument. If present, represents first argument of the rule.",
        Arity = ArgumentArity.ZeroOrOne
    };

    private static readonly Option<string> fileOption = new("--file")
    {
        Description = "The path to the Makefile to use.",
        Required = false
    };
    
    private readonly ILogger<TellCommand> logger;

    public TellCommand(ILogger<TellCommand> logger) : base("tell", "Greet a person by name.")
    {
        Add(firstArgument);
        Add(secondArgument);
        Add(thirdArgument);
        Add(fileOption);

        this.logger = logger;
    }

    public static RuleRunParams ToRunRuleParams(ParseResult parseResult)
    {
        var first = parseResult.GetValue(firstArgument);
        var second = parseResult.GetValue(secondArgument);
        var third = parseResult.GetValue(thirdArgument);
        var file = parseResult.GetValue(fileOption);

        if (third is not null)
        {
            return Case3Args.GetRuleRunParams(first!, second!, third, file, parseResult.UnmatchedTokens);
        }
        if (second is not null)
        {
            return Case2Args.GetRuleRunParams(first!, second!, file, parseResult.UnmatchedTokens);
        }
        if (first is not null)
        {
            return Case1Args.GetRuleRunParams(first!, file, parseResult.UnmatchedTokens);
        }

        return Case0Args.GetRuleRunParams(file, parseResult.UnmatchedTokens);
    }
}