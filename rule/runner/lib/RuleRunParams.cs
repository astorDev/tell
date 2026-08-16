namespace Tell;

public record RuleRunParams(
    Rule Rule,
    string WorkingDirectory,
    IReadOnlyList<string> Args
)
{
    override public string ToString() => $"RuleRunParams\n{Rule}\nWorkingDirectory: {WorkingDirectory}\nArgs: [{string.Join(", ", Args)}])";
}
