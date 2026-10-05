namespace Nishe;

public sealed class ArgsInjector(RootCommand root)
{
    public void Apply(TokenizedRootCommandArgs tokenizedArgs, List<PreprocessorRegistration> preprocessors)
    {
        foreach (var registration in preprocessors.OrderBy(item => item.Position is null))
            Apply(tokenizedArgs, registration);
    }

    public void Apply(TokenizedRootCommandArgs tokenizedArgs, PreprocessorRegistration registration)
    {
        if (tokenizedArgs.ShouldSkip(registration)) return;
        var injection = GetInjection(tokenizedArgs.Entries, registration);
        if (injection is not null) tokenizedArgs.Entries.InsertInjection(registration.Position, injection);
    }

    public ArgsInjection? GetInjection(
        List<ArgsEntry> entries,
        PreprocessorRegistration registration)
    {
        var positionals = entries.GetPositionals();
        var candidateIndex = GetCandidateIndex(entries, positionals, registration.Position);
        var candidate = entries.GetCandidate(candidateIndex);
        var argument = registration.Preprocessor.GetArgumentToInject(candidate);
        return argument is null ? null : new ArgsInjection(positionals, candidateIndex, argument);
    }

    public int GetCandidateIndex(
        List<ArgsEntry> entries,
        List<int> positionals,
        int? position)
    {
        var subcommandIndex = position is null ? entries.GetSubcommandIndex() : -1;
        if (subcommandIndex >= 0) return subcommandIndex;
        return positionals.GetPositionalIndex(position ?? root.Arguments.Count);
    }

}

public record ArgsInjection(List<int> Positionals, int CandidateIndex, string Argument);

public static class ArgsInjectionHelper
{
    public static void InsertInjection(
        this List<ArgsEntry> entries,
        int? position,
        ArgsInjection injection)
    {
        var insertIndex = entries.GetInsertionIndex(injection, position);
        var isArgument = position is not null;
        var tokenType = isArgument ? TokenType.Argument : TokenType.Command;
        var entry = new ArgsEntry(injection.Argument, tokenType, isArgument, isArgument);
        entries.Insert(insertIndex, entry);
    }

    public static int GetInsertionIndex(
        this List<ArgsEntry> entries,
        ArgsInjection injection,
        int? position)
    {
        if (injection.CandidateIndex >= 0) return injection.CandidateIndex;
        if (position is null) return entries.GetLastRootOwnedIndex() + 1;
        if (injection.Positionals.Count > 0) return injection.Positionals[^1] + 1;
        return entries.GetFirstNonDirectiveIndex();
    }
}