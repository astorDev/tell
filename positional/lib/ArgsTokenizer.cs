namespace Nishe;

public sealed class ArgsTokenizer(RootCommand root)
{
    public TokenizedRootCommandArgs Tokenize(string[] args)
    {
        var parsed = Parse(args);
        var entries = parsed.TokenizeEntries();
        var helpRequested = IsHelpRequested(args);
        return new TokenizedRootCommandArgs(entries, helpRequested);
    }

    public ParseResult Parse(string[] args) => ((System.CommandLine.RootCommand)root).Parse(args);

    public bool IsHelpRequested(string[] args)
    {
        var helpAliases = root.GetHelpAliases();
        return args.Any(helpAliases.Contains);
    }
}

public sealed class ArgsTokenBuilder(HashSet<Token> optionTokens)
{
    public List<ArgsEntry> Entries { get; } = [];
    public bool PositionalZone { get; set; } = true;
    public bool AfterDoubleDash { get; set; }

    public List<ArgsEntry> Build(IEnumerable<Token> tokens)
    {
        foreach (var token in tokens) Add(token);
        return Entries;
    }

    public void Add(Token token)
    {
        var isArgument = token.Type == TokenType.Argument && !optionTokens.Contains(token);
        var isUnknownOption = token.IsUnknownOption(isArgument, AfterDoubleDash);
        PositionalZone &= token.Type != TokenType.Command && !isUnknownOption;
        AfterDoubleDash |= token.Type == TokenType.DoubleDash;
        var entry = new ArgsEntry(token.Value, token.Type, PositionalZone && isArgument, PositionalZone);
        Entries.Add(entry);
    }
}

public record TokenizedRootCommandArgs(List<ArgsEntry> Entries, bool HelpRequested);

