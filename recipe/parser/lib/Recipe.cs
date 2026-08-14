using System.Text;

namespace Tell;

public record Recipe(
    RecipeFragment[] Fragments,
    TextSpan LineEnd
)
{
    public static readonly TextParser<Recipe> Parser =
        from t in Tab.Parser
        from f in RecipeFragment.Parser.Many()
        from le in NewLine.SpanParser.OptionalOrDefault()
        select new Recipe(f, le);

    public const string TokenKind = "Recipe";
    public static readonly TextParser<TextSpan> SpanParser = Span.MatchedBy(Parser);

    public string ToCommandString(IReadOnlyDictionary<string, string> variables)
    {
        var sb = new StringBuilder();
        foreach (var fragment in Fragments)
        {
            sb.Append(fragment.ToCommandFragment(variables));
        }
        return sb.ToString();
    }
}

public static class RecipeExtensions
{
    public static TokenizerBuilder<T> MatchRecipe<T>(this TokenizerBuilder<T> builder, T kind) => builder.Match(Recipe.SpanParser, kind);
    public static TokenizerBuilder<string> MatchRecipe(this TokenizerBuilder<string> builder) => builder.MatchRecipe(Recipe.TokenKind);
}