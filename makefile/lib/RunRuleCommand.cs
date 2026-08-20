using System.CommandLine;

namespace Tell;

public class RunRuleCommand : Command
{    
    public VarUseCommandParams Parameters { get; }

    RunRuleCommand(string name, VarUseCommandParams parameters) 
        : base(name, $"Run the rule '{name}'")
    {
        this.Parameters = parameters;

        if (parameters.Argument is not null) Add(parameters.Argument.Value);
        foreach (var option in parameters.Options) Add(option.Value);
    }

    public static IEnumerable<VarUse> UsedVariables(IEnumerable<Recipe> recipes) => recipes
        .SelectMany(recipe => recipe.Fragments)
        .Where(f => f.VarUse is not null)
        .Select(f => f.VarUse!)
        .DistinctBy(vu => vu!.Identifier.Value);

    public static RunRuleCommand From(Rule rule)
    {
        var usedVariables = UsedVariables(rule.Recipes);
        return new(
            rule.Target.Identifier.Value,
            VarUseCommandParams.From(usedVariables)
        );
    }
}