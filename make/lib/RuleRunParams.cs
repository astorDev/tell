namespace Tell;

public record RuleRunParams(
    Rule Rule,
    Makefile Doc,
    string WorkingDirectory,
    IReadOnlyList<string> Args
)
{
    override public string ToString() => $"RuleRunParams\n{Rule}\nWorkingDirectory: {WorkingDirectory}\nArgs: [{string.Join(", ", Args)}])";

    public static RuleRunParams From(Makefile doc, string? ruleName, string workingDirectory, IReadOnlyList<string> args)
    {
        var rule = ruleName != null ? doc.GetRule(ruleName) : doc.FirstRule;
        return new(
            rule,
            doc,
            workingDirectory, 
            args
        );
    }
}
