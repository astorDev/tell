using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace Tell;

public static class RecipeTokens
{
    public static readonly TextParser<TextSpan> VarOpen = Span.EqualTo("$(");
    public static readonly TextParser<TextSpan> VarClose = Span.EqualTo(")");
    public static readonly TextParser<TextSpan> Word = Span.Regex(@"[A-Za-z0-9_\-./]+");
    public static readonly TextParser<TextSpan> Comment = Span.Regex(@"#[^\n]*");
    public static readonly TextParser<TextSpan> Tab = Span.EqualTo("    ").Or(Span.EqualTo("\t"));
    public static readonly TextParser<TextSpan> NewLine = Span.EqualTo("\n").Or(Span.EqualTo("\r\n"));

    internal static bool IsTab(this Token<string> token) => token.Kind == nameof(Tab);
    internal static bool IsVarOpen(this Token<string> token) => token.Kind == nameof(VarOpen);
    internal static bool IsVarClose(this Token<string> token) => token.Kind == nameof(VarClose);
    internal static bool IsWord(this Token<string> token) => token.Kind == nameof(Word);
    internal static bool IsComment(this Token<string> token) => token.Kind == nameof(Comment);
    internal static bool IsNewLine(this Token<string> token) => token.Kind == nameof(NewLine);

    public static readonly Tokenizer<string> Tokenizer = 
        new TokenizerBuilder<string>()
            .Match(Tab, nameof(Tab))
            .Match(NewLine, nameof(NewLine))
            .Ignore(Span.WhiteSpace)
            .Match(Comment, nameof(Comment))
            .Match(VarOpen, nameof(VarOpen))
            .Match(VarClose, nameof(VarClose))
            .Match(Word, nameof(Word))
            .Build();
}