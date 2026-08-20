namespace Tell;

internal class Case1Args
{
    internal static RuleRunParams GetRuleRunParams(string firstArgument, string? file, IReadOnlyList<string> unmatchedTokens)
    {
        return WorkingDirectory.TryUse(firstArgument, out var workingDirectory)
            ? FirstIsWorkingDirectory(workingDirectory, file, unmatchedTokens)
            : FirstArgIsNotWorkingDirectory(workingDirectory, firstArgument, file, unmatchedTokens);
    }

    private static RuleRunParams FirstArgIsNotWorkingDirectory(WorkingDirectory workingDirectory, string firstArgument, string? file, IReadOnlyList<string> unmatchedTokens)
    {
        var found = workingDirectory.GetMakefile(file);
        if (found.Doc.Rules.TryGetValue(firstArgument, out var rule))
        {
            return new RuleRunParams(rule, found.Doc.Assignments, workingDirectory.Path, unmatchedTokens);
        }

        var anyArgs = found.Doc.FirstRule.VarUses.Any();
        if (!anyArgs)
        {
            throw new ArgumentException($"First rule in `{found.Path}` has no arguments, so first positional argument `{firstArgument}` can not be used for it. It couldn't be used as a target either, since no matching target exist in the Makefile.");
        }

        return new RuleRunParams(found.Doc.FirstRule, found.Doc.Assignments, workingDirectory.Path, [firstArgument, .. unmatchedTokens]);
    }

    public static RuleRunParams FirstIsWorkingDirectory(WorkingDirectory workingDirectory, string? file, IReadOnlyList<string> unmatchedTokens)
    {
        var found = workingDirectory.GetMakefile(file);
        return new RuleRunParams(found.Doc.FirstRule, found.Doc.Assignments, workingDirectory.Path, unmatchedTokens);
    }
}