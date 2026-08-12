using System.Text;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace Playground.TextTemplate;

public class CurlyBrackets
{
    public const string OpenSymbol = "{";
    public const string CloseSymbol = "}";

    public static readonly TextParser<TextSpan> OpenSpanParser = Span.EqualTo(OpenSymbol);
    public static readonly TextParser<TextSpan> CloseSpanParser = Span.EqualTo(CloseSymbol);
}

public class Identifier
{
    public static readonly TextParser<char> RestCharParser = 
        Character.LetterOrDigit
            .Or(Character.EqualTo('_'))
            .Or(Character.EqualTo('-'));

    public static readonly TextParser<string> TextParser =
        from first in Character.Letter
        from rest in RestCharParser.Many()
        select first + new string(rest);

    public static readonly TextParser<TextSpan> SpanParser = Span.MatchedBy(TextParser);
}

public class Placeholder
{
    public const string TokenKey = "Placeholder";

    public static readonly TextParser<TextSpan> SpanParser = 
        from open in CurlyBrackets.OpenSpanParser
        from content in Identifier.SpanParser
        from close in CurlyBrackets.CloseSpanParser
        select content;

    public static string GetIdentifier(Token<string> token) => token.ToStringValue().TrimStart('{').TrimEnd('}');
    public static string Replace(Token<string> token, IReadOnlyDictionary<string, string> replacement)
    {
        var identifier = GetIdentifier(token);
        return replacement[identifier];
    }
}

[TestClass]
public class Tokenization
{
    public static readonly Tokenizer<string> V1Tokenizer = new TokenizerBuilder<string>()
        .Match(Span.Except(CurlyBrackets.OpenSymbol), "Text")
        .Match(Placeholder.SpanParser, Placeholder.TokenKey)
        .Build();

    public const string Happy = 
"""
Hello, {name}

This is a long text.    We also keep spaces and new lines.

    Today is: {today}
""";

    [TestMethod]
    public void V1Happy()
    {
        var tokens = V1Tokenizer.Tokenize(Happy);
        foreach (var token in tokens) Console.WriteLine(token);
    }

    [TestMethod]
    public void V1SpaceInPlaceholder()
    {
        var example = "Hello, {name with space}";

        var tokens = V1Tokenizer.TryTokenize(example);
        tokens.HasValue.ShouldBeFalse();
        
        Console.WriteLine(tokens.ErrorMessage);
    }

    [TestMethod]
    public void V1UnclosedPlaceholder()
    {
        var example = "Hello, {name";

        var tokens = V1Tokenizer.TryTokenize(example);
        tokens.HasValue.ShouldBeFalse();
        
        Console.WriteLine(tokens.ErrorMessage);
    }

    [TestMethod]
    public void V1Full()
    {
        var tokens = V1Tokenizer.Tokenize(Happy);
        var placeholderTokens = tokens.Where(t => t.Kind == "Placeholder").ToList();

        Console.WriteLine($"placeholderTokens: {placeholderTokens.Count}");

        foreach (var token in placeholderTokens) Console.WriteLine(token.ToStringValue());

        var replacements = new Dictionary<string, string>
        {
            { "name", "Egor" },
            { "today", DateTime.Now.ToString("yyyy-MM-dd") }
        };

        var result = new StringBuilder();

        foreach (var token in tokens)
        {
            var text = TextForToken(token, replacements);
            result.Append(text);
        }

        Console.WriteLine($"result: {result}");
    }

    private static string TextForToken(Token<string> token, IReadOnlyDictionary<string, string> replacements) => token.Kind switch
    {
        "Text" => token.ToStringValue(),
        Placeholder.TokenKey => Placeholder.Replace(token, replacements),
        _ => throw new InvalidOperationException("Unexpected token kind: " + token.Kind)
    };
}