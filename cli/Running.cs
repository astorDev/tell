using Tell;

public static class Running
{
    public static void HandleTellRunning(this Command tell, TellContext context, RecipeRunner runner)
    {
        tell.HandleRunning(context.Makefile.FirstRule, context, runner);

        foreach (var rule in context.Makefile.Rules.Values)
        {
            var ruleCommand = new RuleInfoCommand(rule);
            ruleCommand.AddLoggingCliOptions();

            ruleCommand.HandleRunning(rule, context, runner);

            tell.Add(ruleCommand);
        }
    }

    public static void HandleRunning(this Command command, Rule rule, TellContext context, RecipeRunner runner)
    {
        var ruleSymbols = ReplacementSymbols.AllFor(rule, context.Makefile.Assignments);
        command.Add(ruleSymbols);

        command.SetAction(async parseResult =>
        {
            var replacements = ruleSymbols.GetMaterializedReplacements(parseResult);
            await runner.RunAll(rule.Recipes, context.WorkingDirectory.Path, replacements);
        });
    }
}