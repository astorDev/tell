namespace Tell;

public record MakefileAssignmentOperator(Token<MakefileTokenKind> Operator)
{
    public static readonly TokenListParser<MakefileTokenKind, Token<MakefileTokenKind>> Parser =
        Token.EqualTo(MakefileTokenKind.Equals)
            .Or(Token.EqualTo(MakefileTokenKind.ColonEquals))
            .Or(Token.EqualTo(MakefileTokenKind.QuestionEquals))
            .Or(Token.EqualTo(MakefileTokenKind.BangEquals))
            .Or(Token.EqualTo(MakefileTokenKind.PlusEquals));
}