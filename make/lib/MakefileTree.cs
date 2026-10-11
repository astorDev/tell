namespace Tell;

public record MakefileTree(
    IReadOnlyDictionary<string, RuleElement> RuleTrees,
    IReadOnlyDictionary<string, Assignment> Assignments
)
{
    public static MakefileTree From(MakefileElement parsed)
    {
        var ruleTrees = parsed.Rules.ToDictionary(rule => rule.Target.Identifier.Value);
        var assignments = parsed.Assignments.ToDictionary(assignment => assignment.Target.Value);
        return new MakefileTree(ruleTrees, assignments);
    }
}
