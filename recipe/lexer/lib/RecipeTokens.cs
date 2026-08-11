global using Superpower;
global using Superpower.Model;
global using Superpower.Parsers;
global using Superpower.Tokenizers;

namespace Tell;

public static class RecipeTokens
{
    public const string VarOpenSymbols = "$(";
    public const string VarCloseSymbols = ")";

    public static readonly TextParser<TextSpan> VarOpen = Span.EqualTo("$(");
    public static readonly TextParser<TextSpan> VarClose = Span.EqualTo(")");
    public static readonly TextParser<TextSpan> Text = Span.Regex(@"(?:(?!\$\(|\)).)+");
    public static readonly TextParser<TextSpan> Comment = Span.Regex(@"#[^\n]*");
    public static readonly TextParser<TextSpan> Tab = Span.EqualTo("    ").Or(Span.EqualTo("\t"));
    public static readonly TextParser<TextSpan> NewLine = Span.EqualTo("\n").Or(Span.EqualTo("\r\n"));

    public static readonly TokenListParser<string, Token<string>> VarOpenParsed = Token.EqualTo(nameof(VarOpen));
    public static readonly TokenListParser<string, Token<string>> VarCloseParsed = Token.EqualTo(nameof(VarClose));
    public static readonly TokenListParser<string, Token<string>> WordParsed = Token.EqualTo(nameof(Text));
    public static readonly TokenListParser<string, Token<string>> CommentParsed = Token.EqualTo(nameof(Comment));
    public static readonly TokenListParser<string, Token<string>> TabParsed = Token.EqualTo(nameof(Tab));
    public static readonly TokenListParser<string, Token<string>> NewLineParsed = Token.EqualTo(nameof(NewLine));

    public static readonly Tokenizer<string> Tokenizer = 
        new TokenizerBuilder<string>()
            .Match(Tab, nameof(Tab))
            .Match(NewLine, nameof(NewLine))
            .Match(Comment, nameof(Comment))
            .Match(VarOpen, nameof(VarOpen))
            .Match(VarClose, nameof(VarClose))
            .Match(Text, nameof(Text))
            .Build();
}