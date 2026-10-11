namespace Tell;

public record MakefileTree(
    IReadOnlyDictionary<string, RuleTree> RuleTrees,
    IReadOnlyDictionary<string, Assignment> Assignments
)
{
    public static MakefileTree From(MakefileSyntax parsed)
    {
        var ruleTrees = parsed.Rules.ToDictionary(rule => rule.Target.Identifier.Value);
        var assignments = parsed.Assignments.ToDictionary(assignment => assignment.Target.Value);
        return new MakefileTree(ruleTrees, assignments);
    }
}
