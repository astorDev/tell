namespace Tell;

public record MakefileBlanks(MakefileNewLine? NewLines, MakefileComment? Comments)
{
    public static readonly TokenListParser<MakefileTokenKind, MakefileBlanks> Parser =
        (from newLines in MakefileNewLine.Parser
         from comments in MakefileComment.Parser
         select new MakefileBlanks(newLines, comments)).Try();
}

public record MakefileNewLine(Token<MakefileTokenKind> NewLine)
{
    public static readonly TokenListParser<MakefileTokenKind, MakefileNewLine> Parser =
        from nl in Token.EqualTo(MakefileTokenKind.NewLine)
        select new MakefileNewLine(nl);
}

public record MakefileComment(Token<MakefileTokenKind>[] Tokens)
{
    public static readonly TokenListParser<MakefileTokenKind, MakefileComment> Parser =
        from comment in Token.EqualTo(MakefileTokenKind.Comment)
        select new MakefileComment([ comment ]);
}