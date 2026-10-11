namespace Tell;

public record Makefile(
    IReadOnlyDictionary<string, Rule> Rules,
    IReadOnlyDictionary<string, Assignment> Assignments
)
{
    public static Makefile From(Copaster.File file)
    {
        var tree = MakefileSyntax.Parsing(file).Thrown();
        return From(tree);
    }

    public static Makefile Parse(string fileContent)
    {
        var tree = MakefileSyntax.Parser.Parse(fileContent);
        return From(tree);
    }

    public static Makefile From(MakefileSyntax parsed)
    {
        var tree = MakefileTree.From(parsed);
        return From(tree);
    }

    public static Makefile From(MakefileTree tree)
    {
        var rules = Rule.DictionaryFrom(tree);
        return new Makefile(rules, tree.Assignments);
    }

    public static bool TryParse(Copaster.File file, out Makefile? makefile, out ParseException? error)
    {
        var parsing = MakefileSyntax.Parsing(file);
        return parsing.AsClassicTry(From, out makefile, out error);
    }

    public Rule GetRule(string name)
    {
        if (!Rules.TryGetValue(name, out var rule)) throw new ($"Rule '{name}' not found in Makefile.");
        return rule;
    }

    public Rule FirstRule => Rules.Values.FirstOrDefault() ?? throw new InvalidOperationException("No rules found in Makefile.");
}
