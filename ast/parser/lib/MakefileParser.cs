global using Superpower;
global using Superpower.Model;
global using Superpower.Parsers;

namespace Tell;

public record MakefileDocument(MakefileVariableAssignment[] Assignments, MakefileRule[] Rules, MakefileBlanks[] Blanks)
{
    public static readonly TokenListParser<MakefileTokenKind, MakefileDocument> Parser =
        from items in MakefileVariableAssignment.Parser.Select(a => (object?)a)
            .Or(MakefileRule.Parsers.Select(r => (object?)r))
            .Or(MakefileBlanks.Parser.Value((object?)null))
            .Many()
        select new MakefileDocument(
            items.OfType<MakefileVariableAssignment>().ToArray(),
            items.OfType<MakefileRule>().ToArray(),
            items.OfType<MakefileBlanks>().ToArray()
        );

    public static MakefileDocument Parse(string input)
    {
        var tokens = MakefileLexer.Tokenizer.Tokenize(input);
        return Parser.Parse(tokens);
    }
}

public class MakefileParser
{
    public static readonly TokenListParser<MakefileTokenKind, Token<MakefileTokenKind>> Word =
        Token.EqualTo(MakefileTokenKind.Word);
}
