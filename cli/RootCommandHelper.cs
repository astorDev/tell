using System.CommandLine.Help;

namespace Tell;

public static class RootCommandHelper
{
    public static readonly HelpOption HelpOption = new();
    public static readonly VersionOption VersionOption = new();

    public static string? OptionalHelpToken(this ParseResult parseResult)
    {
        var helpToken = parseResult.Tokens.FirstOrDefault(t => HelpOption.Name == t.Value || HelpOption.Aliases.Contains(t.Value));
        return helpToken?.Value;
    }

    public static string? OptionalVersionToken(this ParseResult parseResult)
    {
        var versionToken = parseResult.Tokens.FirstOrDefault(t => VersionOption.Name == t.Value || VersionOption.Aliases.Contains(t.Value));
        return versionToken?.Value;
    }

    public static string[] GetInfoTokens(this ParseResult parseResult)
    {
        var helpToken = parseResult.OptionalHelpToken();
        var versionToken = parseResult.OptionalVersionToken();

        return new[] { helpToken, versionToken }.Where(t => t is not null).ToArray()!;
    }
}