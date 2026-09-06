namespace Tell;

public class RunRuleCommand : ParseOnlyRunRuleCommand
{
    private readonly RuleRunParams parameters;
    private readonly RuleRunner runner;

    public RunRuleCommand(RuleRunParams parameters, RuleRunner runner) 
        : base(parameters.Rule.Target.Identifier.Value, VarUseCommandParams.From(parameters.Rule.Placeholders))
    {
        this.parameters = parameters;
        this.runner = runner;

        this.SetAction(Execute);
    }

    public async Task Execute(ParseResult parseResult)
    {
        var variables = this.VarUseParams.GetVarValues(parseResult);
        variables = parameters.Doc.Assignments.TransformVariables(variables);

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
        this.VarUseParams.AddTo(this);
    }
}