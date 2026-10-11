using Copaster;

namespace Tell;

public static class RuleCommandExtensions
{
    public static Command MakeRuleCommand(this Command command, Rule rule, Folder workingDirectory, Func<RuleRunParams, int> execute)
    {
        var replacementSymbols = ReplacementSymbols.AllFor(rule);

        command.Add(replacementSymbols);
        command.SetAction(parseResult =>
        {
            var replacementRules = replacementSymbols.Select(rs => rs.GetValue(parseResult));
            var materializedReplacements = replacementRules.Materialize();
            var ruleRunParams = new RuleRunParams(rule, workingDirectory, materializedReplacements);

            return execute(ruleRunParams);
        });
        
        return command;
    }

    public static Command MakeRuleCommand(this Command command, Rule rule, Folder workingDirectory, Func<RuleRunParams, Task<int>> execute)
    {
        var replacementSymbols = ReplacementSymbols.AllFor(rule);

        command.Add(replacementSymbols);
        command.SetAction(async parseResult =>
        {
            var replacementRules = replacementSymbols.Select(rs => rs.GetValue(parseResult));
            var materializedReplacements = replacementRules.Materialize();
            var ruleRunParams = new RuleRunParams(rule, workingDirectory, materializedReplacements);

            return await execute(ruleRunParams);
        });

        return command;
    }

    public static Command MakeRuleCommand(this Command command, Rule rule, Folder workingDirectory, Func<RuleRunParams, Task> execute)
    {
        var replacementSymbols = ReplacementSymbols.AllFor(rule);

        command.Add(replacementSymbols);
        command.SetAction(async parseResult =>
        {
            var replacementRules = replacementSymbols.Select(rs => rs.GetValue(parseResult));
            var materializedReplacements = replacementRules.Materialize();
            var ruleRunParams = new RuleRunParams(rule, workingDirectory, materializedReplacements);

            await execute(ruleRunParams);
        });

        return command;
    }

    public static Command MakeRuleCommand(this Command command, Rule rule, Folder workingDirectory, Action<RuleRunParams> execute)
    {
        var replacementSymbols = ReplacementSymbols.AllFor(rule);

        command.Add(replacementSymbols);
        command.SetAction(parseResult =>
        {
            var replacementRules = replacementSymbols.Select(rs => rs.GetValue(parseResult));
            var materializedReplacements = replacementRules.Materialize();
            var ruleRunParams = new RuleRunParams(rule, workingDirectory, materializedReplacements);

            execute(ruleRunParams);
        });

        return command;
    }
}
