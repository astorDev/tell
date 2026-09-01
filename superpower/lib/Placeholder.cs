namespace Tell;

public record Placeholder(Identifier Identifier)
{
    public const string TokenKey = "Placeholder";

    public string Replace(IReadOnlyDictionary<string, string> replacements, string fallback)
    {
        return replacements.TryGetValue(Identifier.Value, out var replacement) ? replacement : fallback;
    }

    public string Replace(IReadOnlyDictionary<string, string> replacements)
    {
        if (!replacements.TryGetValue(Identifier.Value, out var replacement))
        {
            throw new KeyNotFoundException($"The placeholder '{Identifier.Value}' was not found in the replacements dictionary.");
        }

        return replacement;
    }

    public string ToString(Boundaries boundaries) => $"{boundaries.Opener.Symbol}{Identifier.Value}{boundaries.Closer.Symbol}";
}

public record Boundary(string Symbol)
{
    public TextParser<TextSpan> SpanParser => Span.EqualTo(Symbol);
}

public record Boundaries(Boundary Opener, Boundary Closer)
{
    public static Boundaries FromSymbols(string openerSymbol, string closerSymbol) => new(
        new Boundary(openerSymbol), 
        new Boundary(closerSymbol)
    );

    public TextParser<Placeholder> PlaceholderParser =>
        from opener in Opener.SpanParser
        from identifier in Identifier.Parser
        from closer in Closer.SpanParser
        select new Placeholder(identifier);
}