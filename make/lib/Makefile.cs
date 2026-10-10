using Copaster;

namespace Tell;

public record Makefile(
    IReadOnlyDictionary<string, Rule> Rules,
    IReadOnlyDictionary<string, Assignment> Assignments
)
{
    public static Makefile From(Copaster.File file)
    {
        var tree = MakefileTree.Parsing(file).Thrown();
        return From(tree);
    }

    public static Makefile From(MakefileTree tree)
    {
        var assignments = tree.Assignments.ToDictionary(assignment => assignment.Target.Value, assignment => assignment);
        var rules = tree.Rules.ToDictionary(rule => rule.Name, rule => rule);
        var finalizer = new RuleFinalizer(rules, assignments);
        var finalizedRules = finalizer.FinalizeAll();
        return new Makefile(finalizedRules, assignments);
    }

    public static bool TryParse(Copaster.File file, out Makefile? makefile, out ParseException? error)
    {
        var parsing = MakefileTree.Parsing(file);
        return parsing.AsClassicTry(From, out makefile, out error);
    }

    public Rule GetRule(string name)
    {
        if (!Rules.TryGetValue(name, out var rule)) throw new ($"Rule '{name}' not found in Makefile.");
        return rule;
    }

    public Rule FirstRule => Rules.Values.FirstOrDefault() ?? throw new InvalidOperationException("No rules found in Makefile.");
}

