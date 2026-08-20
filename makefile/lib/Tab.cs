namespace Tell;

public record Tab
{
    public static TextParser<Tab> Parser => SpanParser.Value(new Tab());

    public const string TokenKind = "Tab";
    public static readonly TextParser<TextSpan> SpanParser = Span.EqualTo("\t").Or(Span.EqualTo("    "));
    public static readonly TokenListParser<string, Tab> TokenMatch = Token.EqualTo(TokenKind).Select(_ => new Tab());
}

public static class TabExtensions
{
    public static TokenizerBuilder<string> MatchTabs(this TokenizerBuilder<string> builder) => builder.Match(Tab.SpanParser, Tab.TokenKind);
}