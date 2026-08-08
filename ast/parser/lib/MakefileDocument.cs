global using Superpower;
global using Superpower.Model;
global using Superpower.Parsers;

namespace Tell;

public record MakefileDocument(MakefileVariableAssignment[] Assignments, MakefileRule[] Rules)
{
    public static readonly TokenListParser<MakefileTokenKind, Unit> Blank =
        Token.EqualTo(MakefileTokenKind.NewLine).Value(Unit.Value)
            .Or(Token.EqualTo(MakefileTokenKind.Comment)
                .IgnoreThen(Token.EqualTo(MakefileTokenKind.NewLine))
                .Value(Unit.Value));

    public static readonly TokenListParser<MakefileTokenKind, MakefileDocument> Parser =
        from items in MakefileVariableAssignment.Parser.Select(a => (object?)a)
            .Or(MakefileRule.Parsers.Select(r => (object?)r))
            .Or(Blank.Value((object?)null))
            .Many()
        select new MakefileDocument(
            items.OfType<MakefileVariableAssignment>().ToArray(),
            items.OfType<MakefileRule>().ToArray()
        );

    public static MakefileDocument Parse(string input)
    {
        var tokens = MakefileLexer.Tokenizer.Tokenize(input);
        return Parser.Parse(tokens);
    }
}
