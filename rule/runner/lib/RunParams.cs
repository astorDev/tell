namespace Tell;

public record RuleRunParams(
    Rule Rule,
    string WorkingDirectory,
    IReadOnlyList<string> Args
);
