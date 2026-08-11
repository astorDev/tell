using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace Tell.Vars.Use.Lexer;

public class VarUseTokens
{
    public const string VarOpenSymbols = "$(";
    public const string VarCloseSymbols = ")";

    public static readonly TextParser<TextSpan> VarOpen = Span.EqualTo("$(");
    public static readonly TextParser<TextSpan> VarClose = Span.EqualTo(")");
    public static readonly TextParser<TextSpan> Text = Span.Regex(@"(?:(?!\$\(|\)).)+");

    public static readonly TokenListParser<string, Token<string>> VarOpenParsed = Token.EqualTo(nameof(VarOpen));
    public static readonly TokenListParser<string, Token<string>> VarCloseParsed = Token.EqualTo(nameof(VarClose));
    public static readonly TokenListParser<string, Token<string>> WordParsed = Token.EqualTo(nameof(Text));

    public static readonly Tokenizer<string> Tokenizer = 
        new TokenizerBuilder<string>()
            .Match(VarOpen, nameof(VarOpen))
            .Match(VarClose, nameof(VarClose))
            .Match(Text, nameof(Text))
            .Build();
}
