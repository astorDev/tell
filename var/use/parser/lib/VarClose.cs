using Superpower;
using Superpower.Model;
using Superpower.Parsers;

namespace Tell;

public class VarClose
{
    public const string Symbol = ")";

    public static readonly TextParser<TextSpan> SpanParser = Span.EqualTo(Symbol);
}
