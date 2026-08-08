namespace Tell;

public record MakefileRecipe(Token<MakefileTokenKind>[] Tokens)
{
    public static readonly TokenListParser<MakefileTokenKind, MakefileRecipe> Parser =
        from _ in Token.EqualTo(MakefileTokenKind.Tab)
        from tokens in Token.Matching<MakefileTokenKind>(k => k != MakefileTokenKind.Tab && k != MakefileTokenKind.NewLine, "recipe token").Many()
        from nl in Token.EqualTo(MakefileTokenKind.NewLine)
        select new MakefileRecipe(tokens);
}
