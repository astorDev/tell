global using Superpower;
global using Superpower.Model;
global using Superpower.Parsers;
global using Superpower.Tokenizers;
global using System.CommandLine;

namespace Tell;

public record Makefile(
    IReadOnlyList<DocFragment> Fragments,
    IReadOnlyDictionary<string, Rule> Rules,
    IReadOnlyList<Assignment> Assignments
)
{
    public static readonly TextParser<Makefile> Parser =
        DocFragment.Parser.Many().Select(From);

    public static Makefile Load(string path)
    {
        if (!File.Exists(path)) throw new FileNotFoundException($"Makefile not found at `{path}`.");
        var fileContent = File.ReadAllText(path);
        var doc = Parser.Parse(fileContent);
        return doc;
    }

    public static Makefile From(IReadOnlyList<DocFragment> fragments)
    {
        var rules = fragments
            .Where(f => f.Rule is not null)
            .Select(f => f.Rule!)
            .ToDictionary(r => r.Target.Identifier.Value, r => r);

        var assignments = fragments
            .Where(f => f.Assignment is not null)
            .Select(f => f.Assignment!)
            .ToList();

        return new Makefile(fragments, rules, assignments);
    }

    public Rule GetRule(string name)
    {
        if (!Rules.TryGetValue(name, out var rule)) throw new ($"Rule '{name}' not found in Makefile.");
        return rule;
    }

    public Rule FirstRule => Rules.Values.FirstOrDefault() ?? throw new InvalidOperationException("No rules found in Makefile.");
}