using System.CommandLine;

namespace Tell;

public class RunRuleCommand : ParseOnlyRunRuleCommand
{
    private readonly RuleRunParams parameters;
    private readonly RuleRunner runner;

    public RunRuleCommand(RuleRunParams parameters, RuleRunner runner) 
        : base(parameters.Rule.Target.Identifier.Value, VarUseCommandParams.From(parameters.Rule.VarUses))
    {
        this.parameters = parameters;
        this.runner = runner;

        this.SetAction(Execute);
    }

    public async Task Execute(ParseResult parseResult)
    {
        var variables = this.VarUseParams.GetVarValues(parseResult);

        await runner.Run(
            parameters.Rule.Recipes, 
            parameters.WorkingDirectory, 
            variables
        );
    }
}

public class ParseOnlyRunRuleCommand : Command
{    
    public VarUseCommandParams VarUseParams { get; }

    protected ParseOnlyRunRuleCommand(string name, VarUseCommandParams parameters) 
        : base(name, $"Run the rule '{name}'")
    {
        this.VarUseParams = parameters;

        if (parameters.Argument is not null) Add(parameters.Argument.Value);
        foreach (var option in parameters.Options) Add(option.Value);
    }

    public static IEnumerable<VarUse> UsedVariables(IEnumerable<Recipe> recipes) => recipes
        .SelectMany(recipe => recipe.Fragments)
        .Where(f => f.VarUse is not null)
        .Select(f => f.VarUse!)
        .DistinctBy(vu => vu!.Identifier.Value);

    public static ParseOnlyRunRuleCommand From(Rule rule)
    {
        var usedVariables = UsedVariables(rule.Recipes);
        return new(
            rule.Target.Identifier.Value,
            VarUseCommandParams.From(usedVariables)
        );
    }
}