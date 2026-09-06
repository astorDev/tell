using Superpower;

namespace Tell;

public record UnparsedLine
{
    public static readonly TextParser<UnparsedLine> Parser = 
        from a in Anything.SpanExceptNewLine.OptionalOrDefault()
        from nl in NewLine.SpanParser
        select new UnparsedLine();
}
