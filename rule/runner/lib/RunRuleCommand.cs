using System.CommandLine;
using Superpower;

namespace Tell;

public class RunRuleCommand : Command
{
    private readonly RuleRunner runner;
    private readonly string workingDirectory;
    private readonly Rule rule;
    private readonly VarUseCommandParams parameters;
    
    RunRuleCommand(RuleRunner runner, string workingDirectory, Rule rule, string name, VarUseCommandParams parameters) 
        : base(name, $"Run the rule '{name}'")
    {
        this.runner = runner;
        this.workingDirectory = workingDirectory;
        this.rule = rule;
        this.parameters = parameters;

        if (parameters.Argument is not null) Add(parameters.Argument.Value);
        foreach (var option in parameters.Options) Add(option.Value);
        SetAction(Execute);
    }

    public static IEnumerable<VarUse> UsedVariables(IEnumerable<Recipe> recipes) => recipes
        .SelectMany(recipe => recipe.Fragments)
        .Where(f => f.VarUse is not null)
        .Select(f => f.VarUse!)
        .DistinctBy(vu => vu!.Identifier.Value);

    public static RunRuleCommand From(Rule rule, RuleRunner runner, string workingDirectory)
    {
        var usedVariables = UsedVariables(rule.Recipes);
        return new(
            runner,
            workingDirectory,
            rule,
            rule.Target.Identifier.Value,
            VarUseCommandParams.From(usedVariables)
        );
    }

    public async Task Execute(ParseResult parseResult)
    {
        var argValues = parameters.GetVarValues(parseResult);
        await this.runner.Run(rule, workingDirectory, argValues);
    }
}