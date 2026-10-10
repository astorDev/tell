namespace Nishe;

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
