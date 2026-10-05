namespace Nishe;

public class DefaultSubcommandInjector(RootCommand root, string name) : IArgsPreprocessor
{
    public string? GetArgumentToInject(string? candidateArg)
    {
        EnsureDefaultSubcommandExists();
        return IsSubcommand(candidateArg) ? null : name;
    }

    public void EnsureDefaultSubcommandExists()
    {
        if (!root.Subcommands.Any(command => command.Name == name))
            throw new InvalidOperationException($"Default subcommand '{name}' is not added to the command.");
    }

    public bool IsSubcommand(string? candidateArg) =>
        candidateArg is not null && HasSubcommand(candidateArg);

    public bool HasSubcommand(string candidateArg)
    {
        foreach (var command in root.Subcommands)
            if (command.IsSubcommand(candidateArg)) return true;
        return false;
    }
}