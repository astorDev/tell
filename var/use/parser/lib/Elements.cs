using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace Tell;

public class VarOpen
{
    public const string Symbol = "$(";

    public static readonly TextParser<TextSpan> SpanParser = Span.EqualTo(Symbol);
}

public class VarClose
{
    public const string Symbol = ")";

    public static readonly TextParser<TextSpan> SpanParser = Span.EqualTo(Symbol);
}

public class Identifier
{
    public static readonly TextParser<char> FirstCharParser = 
        Character.Letter
            .Or(Character.EqualTo('_'));

    public static readonly TextParser<char> RestCharParser = 
        Character.LetterOrDigit
            .Or(Character.EqualTo('_'))
            .Or(Character.EqualTo('-'));

    public static readonly TextParser<string> TextParser =
        from first in FirstCharParser
        from rest in RestCharParser.Many()
        select first + new string(rest);

    public static readonly TextParser<TextSpan> SpanParser = Span.MatchedBy(TextParser);
}

public class VarUse
{
    public const string TokenKey = "VarUse";

    public static readonly TextParser<string> Parser = 
        from open in VarOpen.SpanParser
        from content in Identifier.TextParser
        from close in VarClose.SpanParser
        select content;

    public static readonly TextParser<TextSpan> SpanParser = 
        from open in VarOpen.SpanParser
        from content in Identifier.SpanParser
        from close in VarClose.SpanParser
        select content;
}

public static class VarUseExtensions
{
    public static TokenizerBuilder<string> MatchVarUse(this TokenizerBuilder<string> builder) => 
        builder.MatchVarUse(VarUse.TokenKey);

    public static TokenizerBuilder<T> MatchVarUse<T>(this TokenizerBuilder<T> builder, T tokenKey) => 
        builder.Match(VarUse.SpanParser, tokenKey);
}

public class VarOpenEscaped
{
    public const string Symbol = "$$(";
    public const string TokenKey = "VarOpenEscaped";

    public static readonly TextParser<TextSpan> SpanParser = Span.EqualTo(Symbol);
}

public static class VarOpenEscapedExtensions
{
    public static TokenizerBuilder<string> MatchVarOpenEscaped(this TokenizerBuilder<string> builder) => 
        builder.MatchVarOpenEscaped(VarOpenEscaped.TokenKey);

    public static TokenizerBuilder<T> MatchVarOpenEscaped<T>(this TokenizerBuilder<T> builder, T tokenKey) => 
        builder.Match(VarOpenEscaped.SpanParser, tokenKey);
}