namespace Tell;

public static class Case2Args
{
    public static RuleRunParams GetRuleRunParams(string firstArgument, string secondArgument, string? file, IReadOnlyList<string> unmatchedTokens)
    {
        return WorkingDirectory.TryUse(firstArgument, out var workingDirectory)
            ? FirstIsWorkingDirectory(workingDirectory, secondArgument, file, unmatchedTokens)
            : FirstArgIsNotWorkingDirectory(workingDirectory, firstArgument, secondArgument, file, unmatchedTokens);
    }

    public static RuleRunParams FirstIsWorkingDirectory(WorkingDirectory workingDirectory, string secondArgument, string? file, IReadOnlyList<string> unmatchedTokens)
    {
        var found = workingDirectory.GetMakefile(file);
        if (found.Doc.Rules.TryGetValue(secondArgument, out var rule))
        {
            return new RuleRunParams(rule, found.Doc.Assignments, workingDirectory.Path, unmatchedTokens);
        }

        var anyArgumentInFirstRule = found.Doc.FirstRule.Recipes.SelectMany(r => r.Fragments.Where(f => f.VarUse is not null)).Any();
        if (!anyArgumentInFirstRule)
        {
            throw new ArgumentException($"First rule in `{found.Path}` has no arguments, so second positional argument `{secondArgument}` can not be used for it. It couldn't be used as a target either, since no matching target exist in the Makefile.");
        }

        return new RuleRunParams(found.Doc.FirstRule, found.Doc.Assignments, workingDirectory.Path, [secondArgument, .. unmatchedTokens]);
    }

    public static RuleRunParams FirstArgIsNotWorkingDirectory(WorkingDirectory workingDirectory, string firstArgument, string secondArgument, string? file, IReadOnlyList<string> unmatchedTokens)
    {
        var found = workingDirectory.GetMakefile(file);
        if (found.Doc.Rules.TryGetValue(firstArgument, out var rule))
        {
            return new RuleRunParams(rule, found.Doc.Assignments, workingDirectory.Path, [ secondArgument, ..unmatchedTokens ]);
        }

        throw new ArgumentException($@"First positional argument `{firstArgument}` neither works as a working directory nor as a target:
- Working directory `{workingDirectory.SearchPath}` doesn't exists.
- Target `{firstArgument}` is not found in `{found.Path}`.");
    }
}