using Superpower;

namespace Tell;

public record Doc(
    IReadOnlyList<DocFragment> Fragments,
    IReadOnlyDictionary<string, Rule> Rules
)
{
    public static readonly TextParser<Doc> Parser =
        DocFragment.Parser.Many().Select(Doc.From);

    public static Doc From(IReadOnlyList<DocFragment> fragments)
    {
        var rules = fragments
            .Where(f => f.Rule is not null)
            .Select(f => f.Rule!)
            .ToDictionary(r => r.Target.Identifier.Value, r => r);

        return new Doc(fragments, rules);
    }

    public Rule GetRule(string name)
    {
        if (!Rules.TryGetValue(name, out var rule)) throw new ($"Rule '{name}' not found in Makefile.");
        return rule;
    }

    public Rule FirstRule => Rules.Values.FirstOrDefault() ?? throw new InvalidOperationException("No rules found in Makefile.");
}