namespace Tell;

internal class Case0Args
{
    internal static RuleRunParams GetRuleRunParams(string? file, IReadOnlyList<string> unmatchedTokens)
    {
        var workingDirectory = WorkingDirectory.Default;
        var parsing = workingDirectory.MakefileParsing(file);
        var doc = parsing.DocOrThrowParsingError();
        return new RuleRunParams(doc.FirstRule, doc, workingDirectory.Path, unmatchedTokens);
    }
}
