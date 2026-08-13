using System.Text.RegularExpressions;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace Tell;

public class Anything
{
    public static readonly TextParser<TextSpan> TextSpan = Span.Regex(".*");

    public static TextParser<TextSpan> Before(params string[] delimiters)
    {
        var alternatives = string.Join("|", delimiters.Select(Regex.Escape));

        return Span.Regex($@"^(?:(?!{alternatives})[\s\S])+");
    }

    public static Tokenizer<T> DefaultingTokenizer<T>(Action<TokenizerBuilder<T>> configure, T Key)
    {
        var builder = new TokenizerBuilder<T>();
        configure(builder);
        return builder.Match(TextSpan, Key).Build();
    }

    public static Tokenizer<string> Tokenizer(Action<TokenizerBuilder<string>>? configure) => DefaultingTokenizer(configure ?? (_ => { }), nameof(Anything));
}

public static class AnythingExtensions
{
    public static TokenizerBuilder<T> MatchAnything<T>(this TokenizerBuilder<T> builder, T Key) => builder.Match(Anything.TextSpan, Key);
    public static TokenizerBuilder<string> MatchAnything(this TokenizerBuilder<string> builder) => builder.Match(Anything.TextSpan, nameof(Anything));
}