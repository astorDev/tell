namespace Tell;

public record RuleRunParams(
    Rule Rule,
    IReadOnlyList<Assignment> Assignments,
    string WorkingDirectory,
    IReadOnlyList<string> Args
)
{
    override public string ToString() => $"RuleRunParams\n{Rule}\nWorkingDirectory: {WorkingDirectory}\nArgs: [{string.Join(", ", Args)}])";

    public static RuleRunParams From(Doc doc, string? ruleName, string workingDirectory, IReadOnlyList<string> args)
    {
        var rule = ruleName != null ? doc.GetRule(ruleName) : doc.FirstRule;
        return new(
            rule,
            doc.Assignments,
            workingDirectory, 
            args
        );
    }
}
