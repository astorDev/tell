namespace Nishe;

public interface IArgsPreprocessor
{
    public string? GetArgumentToInject(string? candidateArg);
}