using System.Text;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;
using Tell;

namespace Playground.TextTemplate;

[TestClass]
public class Tokenization
{
    public static readonly Tokenizer<string> Tokenizer = new TokenizerBuilder<string>()
        .Match(Span.Except(TextTemplate.CurlyBrackets.Opener.Symbol), "Text")
        .Match(TextTemplate.CurlyBrackets.PlaceholderParser, Placeholder.TokenKey)
        .Build();

    [TestMethod]
    public void Tokens()
    {
        var tokens = Tokenizer.Tokenize(TextTemplate.Happy);
        foreach (var token in tokens) Console.WriteLine(token);
    }

    [TestMethod]
    public void SpaceInPlaceholder()
    {
        var example = "Hello, {name with space}";

        var tokens = Tokenizer.TryTokenize(example);
        tokens.HasValue.ShouldBeFalse();
        
        Console.WriteLine(tokens.ErrorMessage);
    }

    [TestMethod]
    public void UnclosedPlaceholder()
    {
        var example = "Hello, {name";

        var tokens = Tokenizer.TryTokenize(example);
        tokens.HasValue.ShouldBeFalse();
        
        Console.WriteLine(tokens.ErrorMessage);
    }

    [TestMethod]
    public void TokenizedReplacements()
    {
        var tokens = Tokenizer.Tokenize(TextTemplate.Happy);
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

        Console.WriteLine($"result:\n{result}");
    }

    private static string TextForToken(Token<string> token, IReadOnlyDictionary<string, string> replacements) => token.Kind switch
    {
        "Text" => token.ToStringValue(),
        Placeholder.TokenKey => TextTemplate.CurlyBrackets.PlaceholderParser.Parse(token.ToStringValue()).Replace(replacements),
        _ => throw new InvalidOperationException("Unexpected token kind: " + token.Kind)
    };
}

