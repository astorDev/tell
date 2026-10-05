namespace Nishe;

public static class CommandExtensions
{
    public static bool IsSubcommand(this Command command, string candidateArg) =>
        command.Name == candidateArg || command.Aliases.Contains(candidateArg);
}
