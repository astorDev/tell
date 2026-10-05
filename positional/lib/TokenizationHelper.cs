namespace Nishe;

public static class TokenizationHelper
{
    public static List<ArgsEntry> TokenizeEntries(this ParseResult parsed)
    {
        var optionTokens = parsed.GetOptionTokens();
        var builder = new ArgsTokenBuilder(optionTokens);
        var entries = builder.Build(parsed.Tokens);
        return entries;
    }

    public static HashSet<Token> GetOptionTokens(this ParseResult parsed)
    {
        var children = parsed.RootCommandResult.Children;
        var options = children.OfType<OptionResult>();
        var tokens = options.SelectMany(option => option.Tokens);
        return tokens.ToHashSet();
    }

    public static bool IsUnknownOption(this Token token, bool isArgument, bool afterDoubleDash) =>
        isArgument && !afterDoubleDash && token.Value.Length > 1 && token.Value.StartsWith('-');
}

public static class HelpOptionHelper
{
    public static HashSet<string> GetHelpAliases(this RootCommand root)
    {
        var rootOptions = root.Options;
        var helpOptions = rootOptions.OfType<System.CommandLine.Help.HelpOption>();
        List<string> aliases = [];
        foreach (var option in helpOptions) aliases.AddHelpAliases(option);
        return aliases.ToHashSet(StringComparer.Ordinal);
    }

    public static void AddHelpAliases(this List<string> aliases, System.CommandLine.Help.HelpOption option)
    {
        foreach (var alias in option.Aliases) aliases.Add(alias);
        aliases.Add(option.Name);
    }

}
