namespace Tell;

public static class RuleDependencyTraversal
{
    public static IReadOnlyList<Rule> Ordered(Makefile makefile, Rule target)
    {
        var traversal = new RuleDependencyTraversalState(makefile);
        traversal.Visit(target);
        return traversal.OrderedRules;
    }
}

public sealed class RuleDependencyTraversalState(Makefile makefile)
{
    public List<Rule> OrderedRules { get; } = [];
    public HashSet<string> CompletedRules { get; } = [];
    public HashSet<string> ActiveRules { get; } = [];

    public void Visit(Rule rule)
    {
        if (CompletedRules.Contains(rule.Name)) return;
        Start(rule);
        VisitDependencies(rule);
        Complete(rule);
    }

    public void Start(Rule rule)
    {
        if (!ActiveRules.Add(rule.Name))
            throw new InvalidOperationException($"Circular dependency detected for rule '{rule.Name}'.");
    }

    public void VisitDependencies(Rule rule)
    {
        foreach (var dependency in rule.Target.Dependencies)
        {
            var dependencyRule = makefile.GetRule(dependency.Value);
            Visit(dependencyRule);
        }
    }

    public void Complete(Rule rule)
    {
        ActiveRules.Remove(rule.Name);
        CompletedRules.Add(rule.Name);
        OrderedRules.Add(rule);
    }
}
