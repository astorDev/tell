using Tell;

public static class Running
{
    public static void HandleTellRunning(this Command tell, TellContext context, RuleRunner runner)
    {
        tell.MakeRuleCommand(context.Makefile.FirstRule, context, runner.Run);

        foreach (var rule in context.Makefile.Rules.Values)
        {
            var ruleCommand = new RuleInfoCommand(rule);
            ruleCommand.AddLoggingCliOptions();

            ruleCommand.MakeRuleCommand(rule, context, runner.Run);

            tell.Add(ruleCommand);
        }
    }
}