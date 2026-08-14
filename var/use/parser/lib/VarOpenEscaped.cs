using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace Tell;

public record VarOpenEscaped
{
    public const string Symbol = "$$(";
    public const string TokenKey = "VarOpenEscaped";
    public const string EscapedSymbol = "$(";

    public static readonly TextParser<TextSpan> SpanParser = Span.EqualTo(Symbol);
}

public static class VarOpenEscapedExtensions
{
    public static TokenizerBuilder<string> MatchVarOpenEscaped(this TokenizerBuilder<string> builder) => 
        builder.MatchVarOpenEscaped(VarOpenEscaped.TokenKey);

    public static TokenizerBuilder<T> MatchVarOpenEscaped<T>(this TokenizerBuilder<T> builder, T tokenKey) => 
        builder.Match(VarOpenEscaped.SpanParser, tokenKey);
}