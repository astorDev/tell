using Superpower;
using Superpower.Model;
using Superpower.Parsers;

namespace Tell;

public record Identifier(string Value)
{
    public static readonly TextParser<char> FirstCharParser = 
        Character.Letter
            .Or(Character.EqualTo('_'));

    public static readonly TextParser<char> RestCharParser = 
        Character.LetterOrDigit
            .Or(Character.EqualTo('_'))
            .Or(Character.EqualTo('-'));

    public static readonly TextParser<Identifier> Parser =
        from first in FirstCharParser
        from rest in RestCharParser.Many()
        select new Identifier(first + new string(rest));

    public static readonly TextParser<TextSpan> SpanParser = Span.MatchedBy(Parser);
    override public string ToString() => Value;
}
