using Superpower;
using Superpower.Model;
using Superpower.Parsers;

namespace Tell;

public class VarOpen
{
    public const string Trigger = "$";
    public const string Symbol = "$(";

    public static readonly TextParser<TextSpan> SpanParser = Span.EqualTo(Symbol);
}
