namespace Tell;

public record Target(Identifier Identifier)
{
    public static readonly TextParser<Target> Parser =
        from identifier in Identifier.TextParser
        from colon in Colon.Parser
        from _ in NewLine.SpanParser.OptionalOrDefault()
        select new Target(identifier);

    public const string TokenKind = "Target";
    public static readonly TextParser<TextSpan> SpanParser = Span.MatchedBy(Parser);

    override public string ToString() => $"{Identifier}:";
}

public static class TargetExtensions
{
    public static TokenizerBuilder<T> MatchTarget<T>(this TokenizerBuilder<T> builder, T kind) => builder.Match(Target.SpanParser, kind);
    public static TokenizerBuilder<string> MatchTarget(this TokenizerBuilder<string> builder) => builder.MatchTarget(Target.TokenKind);
}