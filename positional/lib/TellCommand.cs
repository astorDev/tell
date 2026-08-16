using System.CommandLine;

namespace Tell;

public class TellCommand : Command
{
    public TellCommand() : base("tell", "Greet a person by name.")
    {
        Add(TellCommandParams.firstArgument);
        Add(TellCommandParams.secondArgument);
        Add(TellCommandParams.thirdArgument);
        Add(TellCommandParams.fileOption);
    }

    private static RuleRunParams RunRuleParamsFrom(ParseResult parseResult)
    {
        var parameters = TellCommandParams.From(parseResult);
        var (first, second, third, file, unmatchedTokens) = parameters;

        if (third is not null)
        {
            return Case3Args.GetRuleRunParams(first!, second!, third, file, unmatchedTokens);
        }
        if (second is not null)
        {
            return Case2Args.GetRuleRunParams(first!, second!, file, unmatchedTokens);
        }
        if (first is not null)
        {
            return Case1Args.GetRuleRunParams(first!, file, unmatchedTokens);
        }

        return Case0Args.GetRuleRunParams(file, unmatchedTokens);
    }

    public RuleRunParams GetRunRuleParams(string[] args)
    {
        var parseResult = Parse(args);
        return RunRuleParamsFrom(parseResult);
    }
}