using Superpower;

namespace Tell;

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
