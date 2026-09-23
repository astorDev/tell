using Microsoft.Extensions.Logging;

namespace Tell;

internal class Case1Args
{
    internal static RuleRunParams GetRuleRunParams(string firstArgument, string? file, IReadOnlyList<string> unmatchedTokens, ILogger logger)
    {
        return WorkingDirectory.TryUse(firstArgument, out var workingDirectory)
            ? FirstIsWorkingDirectory(workingDirectory, file, unmatchedTokens, logger)
            : FirstArgIsNotWorkingDirectory(workingDirectory, firstArgument, file, unmatchedTokens, logger);
    }

    private static RuleRunParams FirstArgIsNotWorkingDirectory(WorkingDirectory workingDirectory, string firstArgument, string? file, IReadOnlyList<string> unmatchedTokens, ILogger logger)
    {
        var found = workingDirectory.GetMakefile(file);
        if (found.Doc.Rules.TryGetValue(firstArgument, out var rule))
        {
            return new RuleRunParams(rule, found.Doc, workingDirectory.Path, unmatchedTokens);
        }

        var firstRule = found.Doc.FirstRule;

        var anyArgs = firstRule.Placeholders.Any();
        if (!anyArgs)
        {
            throw new ArgumentException($"First rule in `{found.Path}` has no arguments, so first positional argument `{firstArgument}` can not be used for it. It couldn't be used as a target either, since no matching target exist in the Makefile.");
        }

        logger.LogTrace("Treating first and only argument ({FirstArgument}) as a value of the first rule ({FirstRule}) first placeholder ({Target})", firstArgument, firstRule.Name, firstRule.Placeholders.First().Identifier);
        return new RuleRunParams(firstRule, found.Doc, workingDirectory.Path, [firstArgument, .. unmatchedTokens]);
    }

    public static RuleRunParams FirstIsWorkingDirectory(WorkingDirectory workingDirectory, string? file, IReadOnlyList<string> unmatchedTokens, ILogger logger)
    {
        var found = workingDirectory.GetMakefile(file);
        return new RuleRunParams(found.Doc.FirstRule, found.Doc, workingDirectory.Path, unmatchedTokens);
    }
}