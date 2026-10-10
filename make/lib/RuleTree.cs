namespace Tell;

public record RuleTree(Target Target, Recipe[] Recipes)
{
    public static readonly Tokenizer<string> Tokenizer =
        new TokenizerBuilder<string>()
            .MatchTarget()
            .MatchRecipe()
            .Build();

    public static readonly TextParser<RuleTree> Parser =
        from target in Target.Parser
        from recipes in Recipe.Parser.Many()
        select new RuleTree(target, recipes);

    public const string TokenKind = "Rule";
    public static readonly TextParser<TextSpan> SpanParser = Span.MatchedBy(Parser);
    public string Name => Target.Identifier.Value;

    public override string ToString() =>
        $"{Target}\n{string.Join("\n", Recipes.Select(recipe => $"  {recipe}"))}";
}

public static class RuleTreeExtensions
{
    public static TokenizerBuilder<T> MatchRuleTree<T>(this TokenizerBuilder<T> builder, T kind) =>
        builder.Match(RuleTree.SpanParser, kind);

    public static TokenizerBuilder<string> MatchRuleTree(this TokenizerBuilder<string> builder) =>
        builder.MatchRuleTree(RuleTree.TokenKind);
}
