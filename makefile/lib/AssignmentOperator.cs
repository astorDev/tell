namespace Tell;

public record AssignmentOperator(
    QuestionEquals? QuestionEquals = null,
    EqualsOperator? EqualsOperator = null
)
{
    public static AssignmentOperator FromQuestionEquals(QuestionEquals questionEquals) => new(QuestionEquals: questionEquals);
    public static AssignmentOperator FromEqualsOperator(EqualsOperator equalsOperator) => new(EqualsOperator: equalsOperator);

    public bool IsQuestionEquals => QuestionEquals is not null;
    public bool IsEqualsOperator => EqualsOperator is not null;

    public static readonly TextParser<AssignmentOperator> QuestionEqualsAsAssignmentOperatorParser =
        QuestionEquals.Parser.Select(FromQuestionEquals);

    public static readonly TextParser<AssignmentOperator> EqualsOperatorAsAssignmentOperatorParser =
        EqualsOperator.Parser.Select(FromEqualsOperator);

    public static readonly TextParser<AssignmentOperator> Parser =
        QuestionEqualsAsAssignmentOperatorParser.Try()
        .Or(EqualsOperatorAsAssignmentOperatorParser);
}

public record QuestionEquals
{
    public const string Symbol = "?=";

    public static readonly TextParser<QuestionEquals> Parser = Span.EqualTo(Symbol).Select(_ => new QuestionEquals());
    public static readonly TextParser<TextSpan> SpanParser = Span.EqualTo(Symbol);
}

public record EqualsOperator
{
    public const string Symbol = "=";

    public static readonly TextParser<EqualsOperator> Parser = Span.EqualTo(Symbol).Select(_ => new EqualsOperator());
    public static readonly TextParser<TextSpan> SpanParser = Span.EqualTo(Symbol);
}
