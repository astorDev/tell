using Superpower;

namespace Tell;

public record UnparsedLine
{
    public static readonly TextParser<UnparsedLine> Parser = 
        from a in Anything.ExceptNewLine.OptionalOrDefault()
        from nl in NewLine.SpanParser
        select new UnparsedLine();
}

public record DocFragment(
    Rule? Rule = null,
    UnparsedLine? UnparsedLine = null
)
{
    public static DocFragment FromRule(Rule rule) => new(Rule: rule);
    public static DocFragment FromUnparsedLine(UnparsedLine idleLine) => new(UnparsedLine: idleLine);

    public static readonly TextParser<DocFragment> UnparsedLineAsDocFragmentParser =
        UnparsedLine.Parser.Select(FromUnparsedLine);

    public static readonly TextParser<DocFragment> RuleAsFragmentParser = 
        Rule.Parser.Select(FromRule);

    public static readonly TextParser<DocFragment> Parser =
        RuleAsFragmentParser.Try()
        .Or(UnparsedLineAsDocFragmentParser);
}

public record Doc(
    DocFragment[] Fragments
)
{
    public static readonly TextParser<Doc> Parser =
        DocFragment.Parser.Many().Select(fragments => new Doc(fragments));
}
