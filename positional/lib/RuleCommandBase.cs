namespace Tell;

public class RuleCommand : RuleInfoCommand
{
    private readonly IReadOnlyList<ReplacementSymbols> replacementSymbols;

    public RuleCommand(Rule rule, IReadOnlyList<ReplacementSymbols> replacementSymbols, Action<IReadOnlyDictionary<string, string>> action) : base(rule)
    {
        replacementSymbols.AddAllTo(this);
        this.replacementSymbols = replacementSymbols;
        this.SetAction((pr) => {
            var r = GetReplacements(pr);
            action(r);
        });
    }

    public IReadOnlyDictionary<string, string> GetReplacements(ParseResult parseResult)
    {
        var replacementRules = replacementSymbols.Select(rs => rs.GetValue(parseResult));
        var materializedReplacements = replacementRules.Materialize();

        return materializedReplacements;
    }
}

public static class RuleCommandExtensions
{
    public static Command MakeRuleCommand(this Command command, Rule rule, TellContext tellContext, Func<RuleRunParams, int> execute)
    {
        var replacementSymbols = ReplacementSymbols.AllFor(rule, tellContext.Makefile.Assignments);

        command.Add(replacementSymbols);
        command.SetAction(parseResult =>
        {
            var replacementRules = replacementSymbols.Select(rs => rs.GetValue(parseResult));
            var materializedReplacements = replacementRules.Materialize();
            var ruleRunParams = new RuleRunParams(rule, tellContext.WorkingDirectory, materializedReplacements);

            return execute(ruleRunParams);
        });
        
        return command;
    }

    public static Command MakeRuleCommand(this Command command, Rule rule, TellContext tellContext, Func<RuleRunParams, Task<int>> execute)
    {
        var replacementSymbols = ReplacementSymbols.AllFor(rule, tellContext.Makefile.Assignments);

        command.Add(replacementSymbols);
        command.SetAction(async parseResult =>
        {
            var replacementRules = replacementSymbols.Select(rs => rs.GetValue(parseResult));
            var materializedReplacements = replacementRules.Materialize();
            var ruleRunParams = new RuleRunParams(rule, tellContext.WorkingDirectory, materializedReplacements);

            return await execute(ruleRunParams);
        });

        return command;
    }

    public static Command MakeRuleCommand(this Command command, Rule rule, TellContext tellContext, Func<RuleRunParams, Task> execute)
    {
        var replacementSymbols = ReplacementSymbols.AllFor(rule, tellContext.Makefile.Assignments);

        command.Add(replacementSymbols);
        command.SetAction(async parseResult =>
        {
            var replacementRules = replacementSymbols.Select(rs => rs.GetValue(parseResult));
            var materializedReplacements = replacementRules.Materialize();
            var ruleRunParams = new RuleRunParams(rule, tellContext.WorkingDirectory, materializedReplacements);

            await execute(ruleRunParams);
        });

        return command;
    }

    public static Command MakeRuleCommand(this Command command, Rule rule, TellContext tellContext, Action<RuleRunParams> execute)
    {
        var replacementSymbols = ReplacementSymbols.AllFor(rule, tellContext.Makefile.Assignments);

        command.Add(replacementSymbols);
        command.SetAction(parseResult =>
        {
            var replacementRules = replacementSymbols.Select(rs => rs.GetValue(parseResult));
            var materializedReplacements = replacementRules.Materialize();
            var ruleRunParams = new RuleRunParams(rule, tellContext.WorkingDirectory, materializedReplacements);

            execute(ruleRunParams);
        });

        return command;
    }
}
