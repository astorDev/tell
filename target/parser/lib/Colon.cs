namespace Tell;

public record Colon
{
    public const string Symbol = ":";
    public static readonly TextParser<Colon> Parser = Character.EqualTo(':').Select(_ => new Colon());

    public static readonly TextParser<TextSpan> SpanParser = Span.MatchedBy(Parser);
    public const string TokenKind = "Colon";
}

public static class ColonExtensions
{
    public static TokenizerBuilder<T> MatchColon<T>(this TokenizerBuilder<T> builder, T kind) => builder.Match(Colon.SpanParser, kind);
    public static TokenizerBuilder<string> MatchColon(this TokenizerBuilder<string> builder) => builder.MatchColon(Colon.TokenKind);
}