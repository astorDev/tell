using Superpower;
using Superpower.Model;

namespace Tell;

public record VariableUse(
    Token<string> Name
)
{
    public static readonly TokenListParser<string, VariableUse> List = 
        from vo in RecipeTokens.VarOpenParsed
        from name in RecipeTokens.WordParsed
        from vc in RecipeTokens.VarCloseParsed
        select new VariableUse(name);
}

public record RecipeElement(
    Token<string>? Word,
    VariableUse? VariableUse
)
{
    public static readonly TokenListParser<string, RecipeElement> Parsed =
        RecipeTokens.WordParsed.Select(word => new RecipeElement(word, null))
        .Or
        (
            VariableUse.List.Select(vu => new RecipeElement(null, vu))
        );
}

public record Recipe(
    RecipeElement[] Elements
)
{
    public static readonly TokenListParser<string, Recipe> Parsed =
        from nl in RecipeTokens.NewLineParsed
        from tabs in RecipeTokens.TabParsed
        from e in RecipeElement.Parsed.Many()
        select new Recipe(e);

    public static Recipe Parse(string recipe)
    {
        var tokens = RecipeTokens.Tokenizer.Tokenize(recipe);
        return Parsed.Parse(tokens);
    }

    public override string ToString()
    {
        var parts = Elements.Select(e => e.Word?.ToStringValue() ?? $"$({e.VariableUse?.Name.ToStringValue()})");
        return string.Join("", parts);
    }
}