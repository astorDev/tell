global using Tell;
global using Superpower.Tokenizers;
using Superpower;
using Superpower.Parsers;

namespace Playground;

[TestClass]
public class SmokeTests
{
    [TestMethod]
    public void Basic()
    {
        var example = "$(name)";

        var identifier = VarUse.Parser.Parse(example);

        Assert.AreEqual("name", identifier);
    }

    [TestMethod]    
    public void Tokenizer()
    {
        var example = "Hello, $(NAME)!";

        var tokenizer = new TokenizerBuilder<string>().MatchVarUse()
            .Match(Span.Except(VarOpen.Symbol), "Text")
            .Build();

        var result = tokenizer.Tokenize(example);

        foreach (var token in result)
        {
            Console.WriteLine(token);
        }
    }

    [TestMethod]
    public void TokenizerWithoutText()
    {
        var example = "Hello, $(NAME)!";

        var tokenizer = new TokenizerBuilder<string>().MatchVarUse().Build();

        var result = tokenizer.TryTokenize(example);

        Console.WriteLine(result);

        result.HasValue.ShouldBeFalse();
    }

    [TestMethod]
    public void TokenizerWithEscapedVarOpen()
    {
        var example = """
        Hello, $$(NAME)! 
        Here's a free falling dollar sign: $ and here's closing parens: )
        And here's a var use: $(NAME), enjoy!
        """;

        var escape = Span.Except(VarOpenEscaped.Symbol).Or(VarOpenEscaped.SpanParser);

        var tokenizer = new TokenizerBuilder<string>()
            .MatchVarOpenEscaped()
            .MatchVarUse()
            .Match(Anything.Before(VarOpen.Symbol, VarOpenEscaped.Symbol), "Text")
            .Build();

        var result = tokenizer.Tokenize(example);

        foreach (var token in result)
        {
            Console.WriteLine(token);
        }
    }
}