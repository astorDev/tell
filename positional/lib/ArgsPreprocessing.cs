namespace Nishe;

public sealed class ArgsPreprocessing(RootCommand root)
{
    private readonly RootCommand root = root;
    private readonly List<PreprocessorRegistration> preprocessors = [];
    private readonly ArgsTokenizer tokenizer = new(root);
    private readonly ArgsInjector injector = new(root);

    public void Add(Argument argument)
    {
        if (argument is IArgsPreprocessor preprocessor)
        {
            var registration = new PreprocessorRegistration(root.Arguments.Count, preprocessor);
            preprocessors.Add(registration);
        }
    }

    public void SetDefaultSubcommand(string name)
    {
        preprocessors.RemoveAll(p => p.Position is null);
        var defaultInjector = new DefaultSubcommandInjector(root, name);
        var registration = new PreprocessorRegistration(null, defaultInjector);
        preprocessors.Add(registration);
    }

    public string[] Preprocess(string[] args)
    {
        if (preprocessors.Count == 0) return args;

        var tokenizedArgs = tokenizer.Tokenize(args);
        injector.Apply(tokenizedArgs, preprocessors);
        var values = tokenizedArgs.Entries.Select(entry => entry.Value);
        return values.ToArray();
    }
}

public record PreprocessorRegistration(int? Position, IArgsPreprocessor Preprocessor);
