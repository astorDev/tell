namespace Tell;

public class TellCommand : RootCommand
{
    public TellCommand(IEnumerable<RunRuleCommand> allCommands, RunRuleCommand defaultRuleCommand) : base("tell")
    {
        Add(TellCommandParams.firstArgument);
        Add(TellCommandParams.secondArgument);
        Add(TellCommandParams.thirdArgument);
        Add(TellCommandParams.fileOption);

        foreach (var command in allCommands)
        {
            Add(command);
        }

        foreach (var option in defaultRuleCommand.Options)
        {
            Add(option);
        }

        SetAction(defaultRuleCommand.Execute);
    }
}