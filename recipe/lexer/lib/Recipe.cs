global using Superpower;
global using Superpower.Model;
global using Superpower.Parsers;
global using Superpower.Tokenizers;

namespace Tell;

public record RecipeLine
{
    public const string Key = "Recipe";
    public const string Regex = @"(\t| {3,}).*";
    public static readonly TextParser<TextSpan> TextSpan = Span.Regex(Regex);
}

public static class RecipeLineExtensions
{
    public static TokenizerBuilder<string> MatchRecipes(this TokenizerBuilder<string> builder) => builder.Match(RecipeLine.TextSpan, RecipeLine.Key);
}