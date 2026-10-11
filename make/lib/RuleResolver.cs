namespace Tell;

public class RuleResolver
{
    public static Rule Resolve(RuleElement tree, MakefileTree inputs, RuleResolutionState state)
    {
        if (state.TryGetResolvedRule(tree.Name, out var rule)) 
        {
            return rule;
        }

        return AddResolvedRule(tree, inputs, state);
    }

    public static Rule AddResolvedRule(RuleElement tree, MakefileTree inputs, RuleResolutionState state)
    {
        if (!state.TryBeginResolving(tree.Name)) throw new InvalidOperationException($"Circular dependency detected for rule '{tree.Name}'.");
        try { return ResolveAndStore(tree, inputs, state); }
        finally { state.EndResolving(tree.Name); }
    }

    public static Rule ResolveAndStore(RuleElement tree, MakefileTree inputs, RuleResolutionState state)
    {
        var dependencies = ResolveDependencies(tree, inputs, state);
        var rule = new Rule(tree.Target.Identifier, dependencies, tree.Recipes, inputs.Assignments);
        state.StoreResolvedRule(rule);
        return rule;
    }

    public static List<Rule> ResolveDependencies(RuleElement tree, MakefileTree inputs, RuleResolutionState state) =>
        tree.Target.Dependencies.Select(dependency => ResolveDependency(tree, dependency, inputs, state)).ToList();

    public static Rule ResolveDependency(RuleElement tree, Identifier dependency, MakefileTree inputs, RuleResolutionState state)
    {
        if (!inputs.RuleTrees.TryGetValue(dependency.Value, out var candidate))
            throw new InvalidOperationException($"Dependency `{dependency.Value}` for rule `{tree.Name}` not found in Makefile.");
        return Resolve(candidate, inputs, state);
    }
}

public class RuleResolutionState
{
    private readonly Dictionary<string, Rule> resolvedRules = [];
    private readonly HashSet<string> rulesBeingResolved = [];

    public IReadOnlyDictionary<string, Rule> ResolvedRules => resolvedRules;

    public bool TryGetResolvedRule(string name, out Rule rule) =>
        resolvedRules.TryGetValue(name, out rule!);

    public bool TryBeginResolving(string name) => rulesBeingResolved.Add(name);

    public void StoreResolvedRule(Rule rule) => resolvedRules[rule.Name] = rule;

    public void EndResolving(string name) => rulesBeingResolved.Remove(name);
}