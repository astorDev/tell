namespace Tell;

public static class DefaultCommandExtensions
{
    public static ParseResult ParseWithDefaultCommand(this Command command, string[] args, string defaultCommand)
    {
        var legitCommands = command.Subcommands.Select(sc => sc.Name).ToArray();

        var parsed = command.Parse(args);

        var parsedCommandName = parsed.CommandResult.Command.Name;
        if (legitCommands.Contains(parsedCommandName))
        {
            return parsed;
        }

        var rootTokenCount = command.GetRootTokenCount(args);
        var newArgs = args.InsertAt(rootTokenCount, defaultCommand);

        return command.Parse(newArgs);
    }

    public static int GetRootTokenCount(this Command command, string[] args)
    {
        var rootOnly = new RootCommand(command.Description ?? "");
        foreach (var symbol in command.Children.OfType<Option>()) rootOnly.Add(symbol);
        foreach (var symbol in command.Children.OfType<Argument>()) rootOnly.Add(symbol);

        var rootOnlyParse = rootOnly.Parse(args);
        var rootTokenCount = args.Length - rootOnlyParse.UnmatchedTokens.Count;

        return rootTokenCount;
    }

    public static string[] InsertAt(this string[] array, int index, string value)
    {
        var newArray = new string[array.Length + 1];
        Array.Copy(array, 0, newArray, 0, index);
        newArray[index] = value;
        Array.Copy(array, index, newArray, index + 1, array.Length - index);
        return newArray;
    }
}