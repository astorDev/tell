using System.CommandLine;

namespace Tell;

public record VarUseCommandParams(
    VarUseArgument? Argument,
    IEnumerable<VarUseOption> Options
)
{
    public static VarUseCommandParams From(IEnumerable<VarUse> varUses) => new(
        VarUseArgument.OptionalFrom(varUses.FirstOrDefault()),
        varUses.Select(VarUseOption.From).ToList()
    );

    public IReadOnlyDictionary<string, string> GetVarValues(ParseResult parseResult)
    {
        var argValues = new Dictionary<string, string>();
        if (Argument is not null)
        {
            var firstArgValue = parseResult.GetValue(this.Argument.Value);
            if (firstArgValue is not null)
            {
                argValues[Argument.VarUse.Identifier.Value] = firstArgValue;
            }
        }

        foreach (var option in Options)
        {
            var optionValue = parseResult.GetValue(option.Value);
            if (optionValue is not null)
            {
                argValues[option.VarUse.Identifier.Value] = optionValue;
            }
        }

        return argValues;
    }
}
