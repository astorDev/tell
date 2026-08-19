global using Superpower;
global using Superpower.Model;
global using Superpower.Parsers;

namespace Tell;

public record Doc(
    IReadOnlyList<DocFragment> Fragments,
    IReadOnlyDictionary<string, Rule> Rules,
    IReadOnlyList<Assignment> Assignments
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

        var assignments = fragments
            .Where(f => f.Assignment is not null)
            .Select(f => f.Assignment!)
            .ToList();

        return new Doc(fragments, rules, assignments);
    }

    public Rule GetRule(string name)
    {
        if (!Rules.TryGetValue(name, out var rule)) throw new ($"Rule '{name}' not found in Makefile.");
        return rule;
    }

    public Rule FirstRule => Rules.Values.FirstOrDefault() ?? throw new InvalidOperationException("No rules found in Makefile.");
}