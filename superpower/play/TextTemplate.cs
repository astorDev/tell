using Tell;

namespace Playground.TextTemplate;

public class TextTemplate
{
    public static readonly Boundaries CurlyBrackets = Boundaries.FromSymbols("{", "}");

    public const string Happy = 
"""
Hello, {name}

This is a long text.    We also keep spaces and new lines.

    Today is: {today}
""";

    public static readonly IReadOnlyDictionary<string, string> Replacements = new Dictionary<string, string>
    {
        { "name", "Egor" },
        { "today", DateTime.Now.ToString("yyyy-MM-dd") }
    };
}