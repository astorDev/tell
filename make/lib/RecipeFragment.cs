using System.Text;

namespace Tell;

public record RecipeFragment(
    string? Literal = null,
    Placeholder? Placeholder = null,
    VarUse.Escape? VarEscape = null
)
{
    public static RecipeFragment FromLiteral(string literal) => new(Literal: literal);
    public static RecipeFragment FromVarUse(Placeholder placeholder) => new(Placeholder: placeholder);
    public static RecipeFragment FromVarEscape(VarUse.Escape varEscape) => new(VarEscape: varEscape);

    public static readonly TextParser<RecipeFragment> VarUseAsFragmentParser = 
        VarUse.Boundaries.PlaceholderParser.Select(vu => FromVarUse(vu));

    public static readonly TextParser<RecipeFragment> LiteralAsFragmentParser = 
        Anything.ExceptNewLineBefore(VarUse.Opener.Trigger).Select(lit => FromLiteral(lit.ToStringValue()));

    public static readonly TextParser<RecipeFragment> VarEscapeAsFragmentParser = 
        VarUse.Escape.SpanParser.Select(ve => FromVarEscape(new VarUse.Escape()));

    public static readonly TextParser<RecipeFragment> Parser =
        VarEscapeAsFragmentParser.Try()
        .Or(VarUseAsFragmentParser.Try())
        .Or(LiteralAsFragmentParser);

    public const string TokenKind = "RecipeFragment";
    public static readonly TokenListParser<string, RecipeFragment> TokenMatch = Token.EqualTo(TokenKind).Select(FromToken);
    public static readonly TextParser<TextSpan> SpanParser = Span.MatchedBy(Parser);

    public static RecipeFragment FromToken(Token<string> token) => Parser.Parse(token.ToStringValue());

    public string ToCommandFragment(IReadOnlyDictionary<string, string> variables)
    {
        if (Literal is not null) return Literal;
        if (Placeholder is not null) return Placeholder.Replace(variables);
        if (VarEscape is not null) return VarUse.Escape.EscapedSymbol;
        throw new InvalidOperationException("Invalid RecipeFragment: all properties are null.");
    }

    override public string ToString()
    {
        if (Literal is not null) return Literal;
        if (Placeholder is not null) return Placeholder.ToString();
        if (VarEscape is not null) return VarEscape.ToString();
        throw new InvalidOperationException("Invalid RecipeFragment: all properties are null.");
    }
}

public static class RecipeFragmentExtensions
{
    public static TokenizerBuilder<T> MatchRecipeFragment<T>(this TokenizerBuilder<T> builder, T kind) => builder.Match(RecipeFragment.SpanParser, kind);
    public static TokenizerBuilder<string> MatchRecipeFragment(this TokenizerBuilder<string> builder) => builder.Match(RecipeFragment.SpanParser, RecipeFragment.TokenKind);

    public static string ToCommandString(this IEnumerable<RecipeFragment> fragments, IReadOnlyDictionary<string, string> variables)
    {
        var sb = new StringBuilder();
        foreach (var fragment in fragments)
        {
            sb.Append(fragment.ToCommandFragment(variables));
        }
        return sb.ToString();
    }
}