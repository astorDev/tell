namespace Tell;

public class RuleFinalizer(
    IReadOnlyDictionary<string, RuleTree> ruleTrees,
    IReadOnlyDictionary<string, Assignment> assignments
)
{
    public Dictionary<string, Rule> FinalizedRules { get; } = [];
    public HashSet<string> ActiveRules { get; } = [];

    public IReadOnlyDictionary<string, Rule> FinalizeAll()
    {
        foreach (var tree in ruleTrees.Values) FinalizeRule(tree);
        return FinalizedRules;
    }

    public Rule FinalizeRule(RuleTree tree) =>
        FinalizedRules.TryGetValue(tree.Name, out var rule) ? rule : BuildRule(tree);

    public Rule BuildRule(RuleTree tree)
    {
        StartRule(tree);
        var dependencies = DependenciesFor(tree);
        var rule = new Rule(tree.Target.Identifier, dependencies, tree.Recipes, assignments);
        return CompleteRule(tree, rule);
    }

    public Rule[] DependenciesFor(RuleTree tree) => tree.Target.Dependencies.Select(ResolveRule).ToArray();

    public Rule ResolveRule(Identifier dependency)
    {
        var tree = GetRuleTree(dependency.Value);
        return FinalizeRule(tree);
    }

    public RuleTree GetRuleTree(string name) =>
        ruleTrees.TryGetValue(name, out var tree)
            ? tree
            : throw new InvalidOperationException($"Rule '{name}' not found in Makefile.");

    public void StartRule(RuleTree tree)
    {
        if (!ActiveRules.Add(tree.Name))
            throw new InvalidOperationException($"Circular dependency detected for rule '{tree.Name}'.");
    }

    public Rule CompleteRule(RuleTree tree, Rule rule)
    {
        ActiveRules.Remove(tree.Name);
        FinalizedRules.Add(tree.Name, rule);
        return rule;
    }
}
