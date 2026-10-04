namespace Nishe;

public class DefaultSubcommandInjector(RootCommand root, string name) : IArgsPreprocessor
{
    public string? GetArgumentToInject(string? candidateArg)
    {
        var subcommands = root.Subcommands;
        if (!subcommands.Any(s => s.Name == name))
        {
            throw new InvalidOperationException($"Default subcommand '{name}' is not added to the command.");
        }

        var isSubcommand = candidateArg is not null
            && subcommands.Any(s => s.Name == candidateArg || s.Aliases.Contains(candidateArg));

        return isSubcommand ? null : name;
    }
}