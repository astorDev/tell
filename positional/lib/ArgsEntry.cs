namespace Nishe;

public record ArgsEntry(string Value, TokenType Type, bool IsPositional, bool IsRootOwned);

public static class ArgsEntryExtensions
{
    public static bool ShouldSkip(this TokenizedRootCommandArgs tokenizedArgs, PreprocessorRegistration registration) =>
        registration.Position is null && tokenizedArgs.HelpRequested;

    public static List<int> GetPositionals(this List<ArgsEntry> entries)
    {
        List<int> positionals = [];
        for (var index = 0; index < entries.Count; index++)
            if (entries[index].IsPositional) positionals.Add(index);
        return positionals;
    }

    public static int GetPositionalIndex(this List<int> positionals, int position) =>
        position < positionals.Count ? positionals[position] : -1;

    public static int GetSubcommandIndex(this List<ArgsEntry> entries) =>
        entries.FindIndex(entry => entry.Type == TokenType.Command);

    public static string? GetCandidate(this List<ArgsEntry> entries, int candidateIndex) =>
        candidateIndex >= 0 ? entries[candidateIndex].Value : null;

    public static int GetLastRootOwnedIndex(this List<ArgsEntry> entries) =>
        entries.FindLastIndex(entry => entry.IsRootOwned);

    public static int GetFirstNonDirectiveIndex(this List<ArgsEntry> entries)
    {
        var index = entries.FindIndex(entry => entry.Type != TokenType.Directive);
        return index >= 0 ? index : entries.Count;
    }
}
