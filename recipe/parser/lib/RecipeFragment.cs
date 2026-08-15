global using Superpower;
global using Superpower.Model;
global using Superpower.Parsers;
global using Superpower.Tokenizers;

namespace Tell;

public record RecipeFragment(
    string? Literal = null,
    VarUse? VarUse = null,
    VarOpenEscaped? VarEscape = null
)
{
    public static RecipeFragment FromLiteral(string literal) => new(Literal: literal);
    public static RecipeFragment FromVarUse(VarUse varUse) => new(VarUse: varUse);
    public static RecipeFragment FromVarEscape(VarOpenEscaped varEscape) => new(VarEscape: varEscape);

    public static readonly TextParser<RecipeFragment> VarUseAsFragmentParser = 
        VarUse.Parser.Select(vu => FromVarUse(vu));

    public static readonly TextParser<RecipeFragment> LiteralAsFragmentParser = 
        Anything.ExceptNewLineBefore(VarOpen.Trigger).Select(lit => FromLiteral(lit.ToStringValue()));

    public static readonly TextParser<RecipeFragment> VarEscapeAsFragmentParser = 
        VarOpenEscaped.SpanParser.Select(ve => FromVarEscape(new VarOpenEscaped()));

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
        if (VarUse is not null) return VarUse.ToCommandFragment(variables);
        if (VarEscape is not null) return VarOpenEscaped.EscapedSymbol;
        throw new InvalidOperationException("Invalid RecipeFragment: all properties are null.");
    }

    override public string ToString()
    {
        if (Literal is not null) return Literal;
        if (VarUse is not null) return VarUse.ToString();
        if (VarEscape is not null) return VarEscape.ToString();
        throw new InvalidOperationException("Invalid RecipeFragment: all properties are null.");
    }
}

public static class RecipeFragmentExtensions
{
    public static TokenizerBuilder<T> MatchRecipeFragment<T>(this TokenizerBuilder<T> builder, T kind) => builder.Match(RecipeFragment.SpanParser, kind);
    public static TokenizerBuilder<string> MatchRecipeFragment(this TokenizerBuilder<string> builder) => builder.Match(RecipeFragment.SpanParser, RecipeFragment.TokenKind);
}