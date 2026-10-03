using System.CommandLine.Parsing;

namespace Playground;

public class AdvancedRootCommand : RootCommand
{
    private readonly Dictionary<int, IArgsPreprocessor> preprocessors = [];

    public void Add(ConditionalArgument<string> conditionalArgument)
    {
        this.preprocessors.Add(0, conditionalArgument);
        base.Add(conditionalArgument);
    }

    public string[] Preprocess(string[] args)
    {
        var effectiveArgs = args.ToList();

        foreach (var preprocessor in preprocessors.OrderBy(kvp => kvp.Key))
        {
            var candidateArg = effectiveArgs.ElementAtOrDefault(preprocessor.Key);
            var argumentToInject = preprocessor.Value.GetArgumentToInject(candidateArg);
            if (argumentToInject is not null)
            {
                effectiveArgs.Insert(preprocessor.Key, argumentToInject);
            }
        }

        return effectiveArgs.ToArray();
    }

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
        var parsed = command.Parse(candidateArg);
        var value = parsed.GetValue(this);
        if (value is null) return fallbackInjection;
        
        var conditionMet = condition(value);
        return conditionMet ? null : fallbackInjection;
    }
}

public interface IArgsPreprocessor
{
    public string? GetArgumentToInject(string? candidateArg);
}
