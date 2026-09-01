using System.Text;
using Superpower;
using Superpower.Parsers;
using Tell;

namespace Playground.TextTemplate;

[TestClass]
public class Parsing
{
    record TemplateFragment(string? Literal = null, Placeholder? Placeholder = null)
    {
        public static TemplateFragment FromLiteral(string literal) => new(Literal: literal);

        public static TextParser<TemplateFragment> LiteralAsFragmentParser
            => Span.Except(TextTemplate.CurlyBrackets.Opener.Symbol).Select(s => FromLiteral(s.ToStringValue()));

        public static TextParser<TemplateFragment> PlaceholderAsFragmentParser
            => TextTemplate.CurlyBrackets.PlaceholderParser.Select(p => new TemplateFragment(null, p));

        public static TextParser<TemplateFragment> Parser
            => LiteralAsFragmentParser.Try()
                .Or(PlaceholderAsFragmentParser);

        public string ToFinalString(IReadOnlyDictionary<string, string> replacements)
        {
            if (Literal is not null) return Literal;
            if (Placeholder is not null) return Placeholder.Replace(replacements);
            throw new InvalidOperationException("TemplateFragment must have either a Literal or a Placeholder.");
        }
    }

    [TestMethod]
    public void E2E()
    {
        var parser = TemplateFragment.Parser.Many().Select(fragments => fragments.ToList());
        var parsed = parser.Parse(TextTemplate.Happy);
        var result = new StringBuilder();
        foreach (var fragment in parsed)
        {
            var text = fragment.ToFinalString(TextTemplate.Replacements);
            result.Append(text);
        }

        Console.WriteLine($"result:\n{result}");
    }
}

