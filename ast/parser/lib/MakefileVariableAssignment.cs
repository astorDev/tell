namespace Tell;

public record MakefileVariableAssignment(Token<MakefileTokenKind> Name, Token<MakefileTokenKind> Operator, Token<MakefileTokenKind>[] Value)
{
    public static readonly TokenListParser<MakefileTokenKind, MakefileVariableAssignment> Parser =
        (from name in Token.EqualTo(MakefileTokenKind.Word)
         from op in MakefileAssignmentOperator.Parser
         from values in Token.EqualTo(MakefileTokenKind.Word).Many()
         from nl in Token.EqualTo(MakefileTokenKind.NewLine)
         select new MakefileVariableAssignment(name, op, values)).Try();
}
