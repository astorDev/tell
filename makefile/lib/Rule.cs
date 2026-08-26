namespace Tell;

public record Rule(
    Target Target,
    Recipe[] Recipes
)
{
    public static readonly Tokenizer<string> Tokenizer = 
        new TokenizerBuilder<string>()
            .MatchTarget()
            .MatchRecipe()
            .Build();

    public static readonly TextParser<Rule> Parser =
        from target in Target.Parser
        from recipes in Recipe.Parser.Many()
        select new Rule(target, recipes);

    public const string TokenKind = "Rule";
    public static readonly TextParser<TextSpan> SpanParser = Span.MatchedBy(Parser);

    public string Name => Target.Identifier.Value;
    public IEnumerable<VarUse> VarUses => Recipes.SelectMany(r => r.VarUses).DistinctBy(vu => vu.Identifier.Value);

    override public string ToString() => $"{Target}\n{string.Join("\n", Recipes.Select(r => $"  {r}"))}";
}

public static class RuleExtensions
{
    public static TokenizerBuilder<T> MatchRule<T>(this TokenizerBuilder<T> builder, T kind) => builder.Match(Rule.SpanParser, kind);
    public static TokenizerBuilder<string> MatchRule(this TokenizerBuilder<string> builder) => builder.MatchRule(Rule.TokenKind);
}