using System.CommandLine.Help;
using Microsoft.Extensions.Logging;

namespace Tell;

public class EntryGate : RootCommand
{
    private readonly ILogger<EntryGate> logger;

    public EntryGate(ILogger<EntryGate> logger) : base("tell")
    {
        Add(TellCommandParams.firstArgument);
        Add(TellCommandParams.secondArgument);
        Add(TellCommandParams.thirdArgument);
        Add(TellCommandParams.fileOption);
        this.logger = logger;
    }

    public RuleRunParams RunRuleParamsFrom(ParseResult parseResult)
    {
        var parameters = TellCommandParams.From(parseResult);
        
        logger.LogDebug("Parsed tell command parameters: {Parameters}, UnmatchedTokens: {UnmatchedTokens}, Tokens: {Tokens}", 
            parameters, 
            parameters.ResuppliedTokens,
            parseResult.Tokens    
        );

        var (first, second, third, file, resuppliedTokens) = parameters;

        if (third is not null)
        {
            logger.LogTrace("Matching tell command with case 3 arguments");
            return Case3Args.GetRuleRunParams(first!, second!, third, file, resuppliedTokens);
        }
        if (second is not null)
        {
            logger.LogTrace("Matching tell command with case 2 arguments");
            return Case2Args.GetRuleRunParams(first!, second!, file, resuppliedTokens);
        }
        if (first is not null)
        {
            logger.LogTrace("Matching tell command with case 1 argument");
            return Case1Args.GetRuleRunParams(first!, file, resuppliedTokens, logger);
        }

        logger.LogTrace("Matching tell command with case 0 arguments");
        return Case0Args.GetRuleRunParams(file, resuppliedTokens);
    }

    public RuleRunParams GetRunRuleParams(string[] args)
    {
        var parseResult = Parse(args);
        return RunRuleParamsFrom(parseResult);
    }
}