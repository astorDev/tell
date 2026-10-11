namespace Tell;

public class RuleCollection(
    IReadOnlyDictionary<string, RuleTree> ruleTrees,
    IReadOnlyDictionary<string, Assignment> assignments
) : Dictionary<string, Rule>
{
    private readonly HashSet<string> rulesBeingResolved = [];

    public static RuleCollection From(
        IReadOnlyDictionary<string, RuleTree> ruleTrees,
        IReadOnlyDictionary<string, Assignment> assignments
    )
    {
        var collection = new RuleCollection(ruleTrees, assignments);
        foreach (var tree in ruleTrees.Values) collection.Resolve(tree);
        return collection;
    }

    public Rule Resolve(RuleTree tree)
    {
        if (TryGetValue(tree.Name, out var rule))
        {
            return rule;
        }
        
        return AddResolvedRule(tree);
    }

    public Rule AddResolvedRule(RuleTree tree)
    {
        if (!rulesBeingResolved.Add(tree.Name)) throw new InvalidOperationException($"Circular dependency detected for rule '{tree.Name}'.");

        var dependencies = new List<Rule>();

        foreach (var dependencySearch in tree.Target.Dependencies)
        {
            if (!ruleTrees.TryGetValue(dependencySearch.Value, out var dependencyCandidate))
                throw new ($"Dependency `{dependencySearch.Value}` for rule `{tree.Name}` not found in Makefile.");

            var dependency = Resolve(dependencyCandidate);
            dependencies.Add(dependency);
        }

        var rule = new Rule(tree.Target.Identifier, dependencies, tree.Recipes, assignments);
        rulesBeingResolved.Remove(tree.Name);
        return this[tree.Name] = rule;
    }
}
