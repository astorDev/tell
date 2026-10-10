namespace Tell;

public record RuleCommandBuildingParams(
    Rule Rule,
    string WorkingDirectory,
    IReadOnlyList<string> Args
)
{
    override public string ToString() => $"RuleRunParams\n{Rule}\nWorkingDirectory: {WorkingDirectory}\nArgs: [{string.Join(", ", Args)}])";

    public static RuleCommandBuildingParams From(Rule rule, string workingDirectory, IReadOnlyList<string> args) =>
        new(rule, workingDirectory, args);
}
