namespace Tell;

public record Tab
{
    public const string Key = "Tab";
    public const string Regex = @"\t| {3,}";
    public static readonly TextParser<TextSpan> TextSpan = Span.Regex(Regex);
}

public static class TabExtensions
{
    public static TokenizerBuilder<string> MatchTabs(this TokenizerBuilder<string> builder) => builder.Match(Tab.TextSpan, Tab.Key);
}