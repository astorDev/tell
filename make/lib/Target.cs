namespace Tell;

public record Target(Identifier Identifier, Identifier[] Dependencies)
{
    public static readonly TextParser<char> DependencyWhitespace =
        Character.EqualTo(' ').Or(Character.EqualTo('\t'));

    public static readonly TextParser<Identifier[]> DependenciesParser =
        from leadingWhitespace in DependencyWhitespace.Many()
        from dependencies in Identifier.Parser.ManyDelimitedBy(DependencyWhitespace.AtLeastOnce().Try())
        from trailingWhitespace in DependencyWhitespace.Many()
        select dependencies;

    public static readonly TextParser<Target> Parser =
        from identifier in Identifier.Parser
        from colon in Colon.Parser
        from dependencies in DependenciesParser
        from _ in NewLine.SpanParser.OptionalOrDefault()
        select new Target(identifier, dependencies);

    public const string TokenKind = "Target";
    public static readonly TextParser<TextSpan> SpanParser = Span.MatchedBy(Parser);

    override public string ToString() => $"{Identifier}: {string.Join(" ", Dependencies)}".TrimEnd();
}

public static class TargetExtensions
{
    public static TokenizerBuilder<T> MatchTarget<T>(this TokenizerBuilder<T> builder, T kind) => builder.Match(Target.SpanParser, kind);
    public static TokenizerBuilder<string> MatchTarget(this TokenizerBuilder<string> builder) => builder.MatchTarget(Target.TokenKind);
}