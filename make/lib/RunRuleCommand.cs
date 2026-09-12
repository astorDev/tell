namespace Tell;

public class RunRuleCommand : ParseOnlyRunRuleCommand
{
    private readonly RuleRunParams parameters;
    private readonly RuleRunner runner;

    public RunRuleCommand(RuleRunParams parameters, RuleRunner runner) 
        : base(parameters.Rule.Target.Identifier.Value, parameters.ToVariablesContext())
    {
        this.parameters = parameters;
        this.runner = runner;

        this.SetAction(Execute);
    }

    public async Task Execute(ParseResult parseResult)
    {
        await runner.Run(
            parameters.Rule.Recipes, 
            parameters.WorkingDirectory, 
            VariablesContext,
            parseResult
        );
    }
}

public class ParseOnlyRunRuleCommand : Command
{    
    public VariablesContext VariablesContext { get; }

    protected ParseOnlyRunRuleCommand(string name, VariablesContext variablesContext) 
        : base(name, $"Run the rule '{name}'")
    {
        this.VariablesContext = variablesContext;
        this.VariablesContext.VarUseParams.AddTo(this);
    }
}