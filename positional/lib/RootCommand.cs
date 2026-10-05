namespace Nishe;

public partial class RootCommand : System.CommandLine.RootCommand
{
    private readonly ArgsPreprocessing argsPreprocessor;

    public RootCommand(string description) : base(description)
    {
        argsPreprocessor = new ArgsPreprocessing(this);
    }

    public new void Add(Argument argument)
    {
        argsPreprocessor.Add(argument);
        base.Add(argument);
    }

    public void SetDefaultSubcommand(string name) => argsPreprocessor.SetDefaultSubcommand(name);

    public string[] Preprocess(string[] args) => argsPreprocessor.Preprocess(args);

    public ParseResult Parse(string args)
    {
        var splitArgs = CommandLineParser.SplitCommandLine(args);
        var parsedArgs = splitArgs.ToArray();
        return Parse(parsedArgs);
    }

    public ParseResult Parse(string[] args)
    {
        var preprocessedArgs = Preprocess(args);
        return base.Parse(preprocessedArgs);
    }
}

public interface IArgsPreprocessor
{
    public string? GetArgumentToInject(string? candidateArg);
}
