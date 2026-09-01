namespace Tell;

public record VarUse
{
    public static readonly Boundaries Boundaries = Boundaries.FromSymbols("$(", ")");

    public record Opener
    {
        public const string Symbol = "$(";
        public const string Trigger = "$";
    }

    public record Escape
    {
        public const string Symbol = "$$(";
        public const string TokenKey = "VarOpenEscaped";
        public const string EscapedSymbol = "$(";

        public static readonly TextParser<TextSpan> SpanParser = Span.EqualTo(Symbol);

        override public string ToString()
        {
            return Symbol;
        }
    }
}
