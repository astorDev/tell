using System.CommandLine.Parsing;

namespace Playground;

public class AdvancedRootCommand : RootCommand
{
    private readonly List<(int? Position, IArgsPreprocessor Preprocessor)> preprocessors = [];

    public new void Add(Argument argument)
    {
        if (argument is IArgsPreprocessor preprocessor)
        {
            preprocessors.Add((Arguments.Count, preprocessor));
        }

        base.Add(argument);
    }

    public new void Add(Command command)
    {
        if (command is IArgsPreprocessor preprocessor)
        {
            preprocessors.Add((null, preprocessor));
        }

        base.Add(command);
    }

    public string[] Preprocess(string[] args)
    {
        if (preprocessors.Count == 0) return args;

        var entries = Tokenize(args);

        foreach (var (position, preprocessor) in preprocessors)
        {
            var positionals = entries
                .Select((entry, index) => (entry, index))
                .Where(x => x.entry.IsPositional)
                .Select(x => x.index)
                .ToList();

            var candidateIndex = FindCandidateIndex(entries, positionals, position ?? Arguments.Count, position is null);
            var candidate = candidateIndex >= 0 ? entries[candidateIndex].Value : null;
            var argumentToInject = preprocessor.GetArgumentToInject(candidate);
            if (argumentToInject is null) continue;

            var insertIndex = candidateIndex >= 0 ? candidateIndex
                : position is null ? entries.FindLastIndex(e => e.IsRootOwned) + 1
                : positionals.Count > 0 ? positionals[^1] + 1
                : entries.FindIndex(e => e.Type != TokenType.Directive) is var first and >= 0 ? first : entries.Count;

            var isArgument = position is not null;
            entries.Insert(insertIndex, new Entry(argumentToInject, isArgument ? TokenType.Argument : TokenType.Command, isArgument, isArgument));
        }

        return entries.Select(e => e.Value).ToArray();
    }

    private static int FindCandidateIndex(List<Entry> entries, List<int> positionals, int position, bool isSubcommand)
    {
        if (isSubcommand)
        {
            var subcommandIndex = entries.FindIndex(e => e.Type == TokenType.Command);
            if (subcommandIndex >= 0) return subcommandIndex;
        }

        return position < positionals.Count ? positionals[position] : -1;
    }

    private List<Entry> Tokenize(string[] args)
    {
        var parsed = base.Parse(args);

        var optionTokens = parsed.RootCommandResult.Children
            .OfType<OptionResult>()
            .SelectMany(o => o.Tokens)
            .ToHashSet();

        var entries = new List<Entry>();
        var positionalZone = true;
        var afterDoubleDash = false;
        foreach (var token in parsed.Tokens)
        {
            var isArgument = token.Type == TokenType.Argument && !optionTokens.Contains(token);

            var isUnknownOption = isArgument && !afterDoubleDash && token.Value.Length > 1 && token.Value.StartsWith('-');

            positionalZone &= token.Type != TokenType.Command && !isUnknownOption;
            afterDoubleDash |= token.Type == TokenType.DoubleDash;

            entries.Add(new Entry(token.Value, token.Type, positionalZone && isArgument, positionalZone));
        }

        return entries;
    }

    private record Entry(string Value, TokenType Type, bool IsPositional, bool IsRootOwned);

    public ParseResult Parse(string args)
    {
        var splitArgs = CommandLineParser.SplitCommandLine(args);
        return Parse(splitArgs.ToArray());
    }

    public ParseResult Parse(string[] args)
    {
        var preprocessedArgs = Preprocess(args);
        return base.Parse(preprocessedArgs);
    }
}

public class ConditionalArgument<T>(string name, Func<T, bool> condition, string fallbackInjection) : Argument<T>(name), IArgsPreprocessor
{
    public string? GetArgumentToInject(string? candidateArg)
    {
        if (candidateArg is null) return fallbackInjection;

        var command = new Command("sniffer") { this };
        var parsed = command.Parse([candidateArg]);
        var value = parsed.GetValue(this);
        if (value is null) return fallbackInjection;
        
        var conditionMet = condition(value);
        return conditionMet ? null : fallbackInjection;
    }
}

public class DefaultSubcommand(string name, string? description = null) : Command(name, description), IArgsPreprocessor
{
    public string? GetArgumentToInject(string? candidateArg)
    {
        var isSiblingSubcommand = candidateArg is not null && Parents
            .OfType<Command>()
            .Any(parent => parent.Subcommands.Any(s => s.Name == candidateArg || s.Aliases.Contains(candidateArg)));

        return isSiblingSubcommand ? null : Name;
    }
}

public interface IArgsPreprocessor
{
    public string? GetArgumentToInject(string? candidateArg);
}
