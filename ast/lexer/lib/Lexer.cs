using Superpower;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace Tell;

public static class MakefileLexer
{
    public static readonly Tokenizer<MakefileTokenKind> Tokenizer =
        new TokenizerBuilder<MakefileTokenKind>()
            .Ignore(Span.Regex(@"(?:(?!    )[ ])+"))
            .Match(Span.Regex(@"#[^\n]*"), MakefileTokenKind.Comment)
            .Match(Span.EqualTo("::"), MakefileTokenKind.DoubleColon)
            .Match(Span.EqualTo(":="), MakefileTokenKind.ColonEquals)
            .Match(Span.EqualTo("?="), MakefileTokenKind.QuestionEquals)
            .Match(Span.EqualTo("!="), MakefileTokenKind.BangEquals)
            .Match(Span.EqualTo("+="), MakefileTokenKind.PlusEquals)
            .Match(Span.EqualTo(":"), MakefileTokenKind.Colon)
            .Match(Span.EqualTo("="), MakefileTokenKind.Equals)
            .Match(Span.EqualTo("$("), MakefileTokenKind.VariableOpener)
            .Match(Span.EqualTo(")"), MakefileTokenKind.VariableCloser)
            .Match(Span.EqualTo("\\"), MakefileTokenKind.Backslash)
            .Match(Span.EqualTo("%"), MakefileTokenKind.Percent)
            .Match(Span.EqualTo("@"), MakefileTokenKind.At)
            .Match(Span.EqualTo(","), MakefileTokenKind.Comma)
            .Match(Span.EqualTo("    "), MakefileTokenKind.Tab)
            .Match(Span.EqualTo("\t"), MakefileTokenKind.Tab)
            .Match(Span.Regex(@"\r?\n"), MakefileTokenKind.NewLine)
            .Match(Span.Regex(@"[A-Za-z0-9_\-./]+"), MakefileTokenKind.Word)
            .Build();
}
