namespace Tell;

public record MakefileRule(Token<MakefileTokenKind> Target, Token<MakefileTokenKind>[] Prerequisites, MakefileRecipe[] Recipes)
{
    public static readonly TokenListParser<MakefileTokenKind, MakefileRule> Parsers =
        (from target in Token.EqualTo(MakefileTokenKind.Word)
         from _ in Token.EqualTo(MakefileTokenKind.Colon).Or(Token.EqualTo(MakefileTokenKind.DoubleColon))
         from prereqs in Token.EqualTo(MakefileTokenKind.Word).Many()
         from nl in Token.EqualTo(MakefileTokenKind.NewLine)
         from recipes in MakefileRecipe.Parser.Many()
         select new MakefileRule(target, prereqs, recipes)).Try();
}