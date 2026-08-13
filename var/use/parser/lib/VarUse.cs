using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace Tell;

public record VarUse(Identifier Identifier)
{
    public const string TokenKey = "VarUse";

    public static readonly TextParser<VarUse> Parser = 
        from open in VarOpen.SpanParser
        from content in Identifier.TextParser
        from close in VarClose.SpanParser
        select new VarUse(content);

    public static readonly TextParser<TextSpan> SpanParser = Span.MatchedBy(Parser);
}

public static class VarUseExtensions
{
    public static TokenizerBuilder<string> MatchVarUse(this TokenizerBuilder<string> builder) => 
        builder.MatchVarUse(VarUse.TokenKey);

    public static TokenizerBuilder<T> MatchVarUse<T>(this TokenizerBuilder<T> builder, T tokenKey) => 
        builder.Match(VarUse.SpanParser, tokenKey);
}
