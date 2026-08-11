using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace Tell;

public class NewLine
{
    public static readonly TextParser<TextSpan> TextSpan = Span.EqualTo("\r\n").Or(Span.EqualTo("\n"));

    public static TokenizerBuilder<T> SplitTokenizerBuilder<T>() => new TokenizerBuilder<T>().IgnoreNewLines();
    public static Tokenizer<T> SplitTokenizer<T>(Action<TokenizerBuilder<T>> configure) 
    {
        var builder = SplitTokenizerBuilder<T>();
        configure(builder);
        return builder.Build();
    }
}

public static class NewLineExtensions
{
    public static TokenizerBuilder<T> IgnoreNewLines<T>(this TokenizerBuilder<T> builder) => builder.Ignore(NewLine.TextSpan);
}