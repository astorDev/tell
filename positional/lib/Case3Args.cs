namespace Tell;

public static class Case3Args
{
    public static RuleRunParams GetRuleRunParams(string firstArgument, string secondArgument, string thirdArgument, string? file, IReadOnlyList<string> unmatchedTokens)
    {
        if (!WorkingDirectory.TryUse(firstArgument, out var workingDirectory))
        {
            throw new ArgumentException($"Directory `{workingDirectory.SearchPath}` doesn't exists. (First positional argument: '{firstArgument}' was used as a working directory, since 3 were passed.)");
        }

        var extracted = workingDirectory.GetMakefile(file);
        if (!extracted.Doc.Rules.TryGetValue(secondArgument, out var rule))
        {
            throw new ArgumentException($"Rule '{secondArgument}' not found in `{extracted.Path}`. (Second positional argument: `{secondArgument}` was used as a target since 3 were provided.)");
        }

        return new RuleRunParams(rule, workingDirectory.Path, [ thirdArgument, ..unmatchedTokens ]);
    }
}