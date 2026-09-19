namespace Tell;

public static class EndOfString
{
    public static readonly TextParser<TextSpan> SpanParser = input =>
        input.IsAtEnd
            ? Result.Value(TextSpan.Empty, input, input)
            : Result.Empty<TextSpan>(input, "end of input");
}