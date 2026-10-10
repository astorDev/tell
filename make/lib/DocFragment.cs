using Superpower;

namespace Tell;

public record DocFragment(
    RuleTree? RuleTree = null,
    Assignment? Assignment = null,
    UnparsedLine? UnparsedLine = null
)
{
    public static DocFragment FromRuleTree(RuleTree ruleTree) => new(RuleTree: ruleTree);
    public static DocFragment FromUnparsedLine(UnparsedLine idleLine) => new(UnparsedLine: idleLine);
    public static DocFragment FromAssignment(Assignment assignment) => new(Assignment: assignment);

    public static readonly TextParser<DocFragment> UnparsedLineAsDocFragmentParser =
        UnparsedLine.Parser.Select(FromUnparsedLine);

    public static readonly TextParser<DocFragment> RuleTreeAsFragmentParser =
        RuleTree.Parser.Select(FromRuleTree);

    public static readonly TextParser<DocFragment> AssignmentAsFragmentParser =
        Assignment.Parser.Select(FromAssignment);

    public static readonly TextParser<DocFragment> Parser =
        RuleTreeAsFragmentParser.Try()
        .Or(AssignmentAsFragmentParser)
        .Or(UnparsedLineAsDocFragmentParser);
}
