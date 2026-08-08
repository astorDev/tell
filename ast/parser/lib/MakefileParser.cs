using Superpower;
using Superpower.Model;
using Superpower.Parsers;

namespace Tell;

public record VariableAssignment(Token<MakefileTokenKind> Name, Token<MakefileTokenKind> Operator, Token<MakefileTokenKind>[] Value);

public record MakefileRecipe(Token<MakefileTokenKind>[] Tokens);

public record MakefileRule(Token<MakefileTokenKind> Target, Token<MakefileTokenKind>[] Prerequisites, MakefileRecipe[] Recipes);

public record MakefileDocument(VariableAssignment[] Assignments, MakefileRule[] Rules);

public class MakefileParser
{
    public static readonly TokenListParser<MakefileTokenKind, Token<MakefileTokenKind>> Word =
        Token.EqualTo(MakefileTokenKind.Word);

    public static readonly TokenListParser<MakefileTokenKind, Token<MakefileTokenKind>> AssignmentOperator =
        Token.EqualTo(MakefileTokenKind.Equals)
            .Or(Token.EqualTo(MakefileTokenKind.ColonEquals))
            .Or(Token.EqualTo(MakefileTokenKind.QuestionEquals))
            .Or(Token.EqualTo(MakefileTokenKind.BangEquals))
            .Or(Token.EqualTo(MakefileTokenKind.PlusEquals));

    public static readonly TokenListParser<MakefileTokenKind, MakefileRecipe> RecipeLine =
        from _ in Token.EqualTo(MakefileTokenKind.Tab)
        from tokens in Token.Matching<MakefileTokenKind>(
            k => k != MakefileTokenKind.Tab && k != MakefileTokenKind.NewLine,
            "recipe token").Many()
        from nl in Token.EqualTo(MakefileTokenKind.NewLine)
        select new MakefileRecipe(tokens);

    public static readonly TokenListParser<MakefileTokenKind, VariableAssignment> Assignment =
        (from name in Word
         from op in AssignmentOperator
         from values in Word.Many()
         from nl in Token.EqualTo(MakefileTokenKind.NewLine)
         select new VariableAssignment(name, op, values)).Try();

    public static readonly TokenListParser<MakefileTokenKind, MakefileRule> Rule =
        (from target in Word
         from _ in Token.EqualTo(MakefileTokenKind.Colon).Or(Token.EqualTo(MakefileTokenKind.DoubleColon))
         from prereqs in Word.Many()
         from nl in Token.EqualTo(MakefileTokenKind.NewLine)
         from recipes in RecipeLine.Many()
         select new MakefileRule(target, prereqs, recipes)).Try();

    public static readonly TokenListParser<MakefileTokenKind, Unit> Blank =
        Token.EqualTo(MakefileTokenKind.NewLine).Value(Unit.Value)
            .Or(Token.EqualTo(MakefileTokenKind.Comment)
                .IgnoreThen(Token.EqualTo(MakefileTokenKind.NewLine))
                .Value(Unit.Value));

    public static readonly TokenListParser<MakefileTokenKind, MakefileDocument> DocumentParser =
        from items in Assignment.Select(a => (object?)a)
            .Or(Rule.Select(r => (object?)r))
            .Or(Blank.Value((object?)null))
            .Many()
        select new MakefileDocument(
            items.OfType<VariableAssignment>().ToArray(),
            items.OfType<MakefileRule>().ToArray()
        );

    public static MakefileDocument Parse(string input)
    {
        var tokens = MakefileLexer.Tokenizer.Tokenize(input);
        return DocumentParser.Parse(tokens);
    }
}
