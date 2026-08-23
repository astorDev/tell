namespace Tell;

public class EffectiveTellCommand : RootCommand
{
    public EffectiveTellCommand(IEnumerable<RunRuleCommand> allCommands, RunRuleCommand defaultRuleCommand) : base("tell")
    {
        foreach (var command in allCommands)
        {
            Add(command);
        }

        foreach (var option in defaultRuleCommand.VarUseParams.Options)
        {
            Add(option.Value);
        }

        foreach (var argument in defaultRuleCommand.Arguments)
        {
            Add(argument);
        }

        SetAction(defaultRuleCommand.Execute);
    }
}

public class InfoTellCommand : RootCommand
{
    public InfoTellCommand(IEnumerable<RunRuleCommand> allCommands, RunRuleCommand defaultRuleCommand) : base("tell")
    {
        Add(TellCommandParams.firstArgument);
        Add(TellCommandParams.secondArgument);
        Add(TellCommandParams.thirdArgument);
        Add(TellCommandParams.fileOption);

        foreach (var command in allCommands)
        {
            Add(command);
        }

        foreach (var option in defaultRuleCommand.VarUseParams.Options)
        {
            Add(option.Value);
        }
    }
}