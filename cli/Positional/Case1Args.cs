using Microsoft.Extensions.Logging;

namespace Tell;

internal class Case1Args
{
    internal static MatchingResult GetRuleRunParams(string firstArgument, string? file, IReadOnlyList<string> unmatchedTokens, ILogger logger)
    {
        return WorkingDirectory.TryUse(firstArgument, out var workingDirectory)
            ? FirstIsWorkingDirectory(workingDirectory, file, unmatchedTokens, logger)
            : FirstArgIsNotWorkingDirectory(workingDirectory, firstArgument, file, unmatchedTokens, logger);
    }

    private static RuleRunParams FirstArgIsNotWorkingDirectory(WorkingDirectory workingDirectory, string firstArgument, string? file, IReadOnlyList<string> unmatchedTokens, ILogger logger)
    {
        var found = workingDirectory.MakefileParsing(file);
        var doc = found.DocOrThrowParsingError();

        if (doc.Rules.TryGetValue(firstArgument, out var rule))
        {
            return new RuleRunParams(rule, doc, workingDirectory.Path, unmatchedTokens);
        }

        var firstRule = doc.FirstRule;

        var anyArgs = firstRule.Placeholders.Any();
        if (!anyArgs)
        {
            throw new ArgumentException($"First rule in `{found.Path}` has no arguments, so first positional argument `{firstArgument}` can not be used for it. It couldn't be used as a target either, since no matching target exist in the Makefile.");
        }

        logger.LogTrace("Treating first and only argument ({FirstArgument}) as a value of the first rule ({FirstRule}) first placeholder ({Target})", firstArgument, firstRule.Name, firstRule.Placeholders.First().Identifier);
        return new RuleRunParams(firstRule, doc, workingDirectory.Path, [firstArgument, .. unmatchedTokens]);
    }

    public static MatchingResult FirstIsWorkingDirectory(WorkingDirectory workingDirectory, string? file, IReadOnlyList<string> unmatchedTokens, ILogger logger)
    {
        var parsing = workingDirectory.MakefileParsing(file);
        return parsing.Match<MatchingResult>(
            onSuccess: doc => new RuleRunParams(doc.FirstRule, doc, workingDirectory.Path, unmatchedTokens),
            onParsingError: ex => new MakeFallbackParams(ex, workingDirectory.Change, RuleName: null, Filename: file)
        );
    }
}

