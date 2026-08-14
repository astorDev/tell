using System.Text;

namespace Tell;

public record Recipe(
    RecipeFragment[] Fragments
)
{
    public static readonly TextParser<Recipe> Parser =
        from t in Tab.Parser
        from f in RecipeFragment.Parser.Many()
        select new Recipe(f.ToArray());

    public static readonly TokenListParser<string, Recipe> TokensParser =
        from t in Tab.TokenMatch
        from f in RecipeFragment.TokenMatch.Many()
        select new Recipe(f);

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