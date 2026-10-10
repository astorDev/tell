namespace Tell;

public static class RuleCommandExtensions
{
    public static Command MakeRuleCommand(this Command command, Rule rule, TellContext tellContext, Func<RuleRunParams, int> execute)
    {
        var replacementSymbols = ReplacementSymbols.AllFor(tellContext.Makefile, rule);

        command.Add(replacementSymbols);
        command.SetAction(parseResult =>
        {
            var replacementRules = replacementSymbols.Select(rs => rs.GetValue(parseResult));
            var materializedReplacements = replacementRules.Materialize();
            var ruleRunParams = new RuleRunParams(tellContext.Makefile, rule, tellContext.WorkingDirectory, materializedReplacements);

            return execute(ruleRunParams);
        });
        
        return command;
    }

    public static Command MakeRuleCommand(this Command command, Rule rule, TellContext tellContext, Func<RuleRunParams, Task<int>> execute)
    {
        var replacementSymbols = ReplacementSymbols.AllFor(tellContext.Makefile, rule);

        command.Add(replacementSymbols);
        command.SetAction(async parseResult =>
        {
            var replacementRules = replacementSymbols.Select(rs => rs.GetValue(parseResult));
            var materializedReplacements = replacementRules.Materialize();
            var ruleRunParams = new RuleRunParams(tellContext.Makefile, rule, tellContext.WorkingDirectory, materializedReplacements);

            return await execute(ruleRunParams);
        });

        return command;
    }

    public static Command MakeRuleCommand(this Command command, Rule rule, TellContext tellContext, Func<RuleRunParams, Task> execute)
    {
        var replacementSymbols = ReplacementSymbols.AllFor(tellContext.Makefile, rule);

        command.Add(replacementSymbols);
        command.SetAction(async parseResult =>
        {
            var replacementRules = replacementSymbols.Select(rs => rs.GetValue(parseResult));
            var materializedReplacements = replacementRules.Materialize();
            var ruleRunParams = new RuleRunParams(tellContext.Makefile, rule, tellContext.WorkingDirectory, materializedReplacements);

            await execute(ruleRunParams);
        });

        return command;
    }

    public static Command MakeRuleCommand(this Command command, Rule rule, TellContext tellContext, Action<RuleRunParams> execute)
    {
        var replacementSymbols = ReplacementSymbols.AllFor(tellContext.Makefile, rule);

        command.Add(replacementSymbols);
        command.SetAction(parseResult =>
        {
            var replacementRules = replacementSymbols.Select(rs => rs.GetValue(parseResult));
            var materializedReplacements = replacementRules.Materialize();
            var ruleRunParams = new RuleRunParams(tellContext.Makefile, rule, tellContext.WorkingDirectory, materializedReplacements);

            execute(ruleRunParams);
        });

        return command;
    }
}
