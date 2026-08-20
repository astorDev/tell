namespace Tell;

internal class Case0Args
{
    internal static RuleRunParams GetRuleRunParams(string? file, IReadOnlyList<string> unmatchedTokens)
    {
        var workingDirectory = WorkingDirectory.Default;
        var found = workingDirectory.GetMakefile(file);
        return new RuleRunParams(found.Doc.FirstRule, found.Doc, workingDirectory.Path, unmatchedTokens);
    }
}
