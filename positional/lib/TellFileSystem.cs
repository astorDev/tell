namespace Tell;

public record TellFileSystem(Folder WorkingDir, Copaster.File File)
{
    public static readonly SymbolsContainer Symbols = new(TellFilename.Option, WorkingDirectory.Argument);
    public record SymbolsContainer(Option<string> FilenameOption, Argument<Folder> ContextArgument);

    public static TellFileSystem From(ParseResult parsed)
    {
        var folder = WorkingDirectory.From(parsed);
        var filename = parsed.GetRequiredValue(TellFilename.Option);
        var file = folder.File(filename);
        if (!file.Exists) throw new FileNotFoundException($"'{Path.GetFullPath(file.Path)}' does not exist.");

        return new TellFileSystem(folder, file);
    }
}

public record WorkingDirectory
{
    public static readonly Argument<Folder> Argument = CreateArgument();

    // Reads the token directly: a non-existing folder is a validation error (so it is not treated as a root token
    // by ParseWithDefaultCommand), and reading the converted value would throw on it.
    public static Folder From(ParseResult parsed)
    {
        var tokens = parsed.GetResult(Argument)?.Tokens;
        if (tokens is not { Count: > 0 }) return new(".");

        var folder = new Folder(tokens[0].Value);
        return folder.Exists ? folder : new(".");
    }

    static Argument<Folder> CreateArgument()
    {
        var argument = new Argument<Folder>("workDir")
        {
            Description = "The working directory.",
            Arity = ArgumentArity.ZeroOrOne,
            DefaultValueFactory = (result) => new("."),
            CustomParser = result =>
            {
                if (result.Tokens.Count == 0) return new(".");

                var token = result.Tokens[0].Value;
                var folder = new Folder(token);
                return folder.Exists ? folder : new(".");
            }
        };

        argument.Validators.Add(result =>
        {
            if (result.Tokens.Count == 0) return;
            if (!new Folder(result.Tokens[0].Value).Exists) result.AddError($"Working directory '{result.Tokens[0].Value}' does not exist.");
        });

        return argument;
    }
}

public class TellFilename
{
    public static readonly Option<string> Option = new("--file")
    {
        DefaultValueFactory = (x) => "Makefile"
    };
}

public static class TellFilesystemExtensions
{
    public static void Add(this Command command, TellFileSystem.SymbolsContainer symbols)
    {
        command.Add(symbols.FilenameOption);
        command.Add(symbols.ContextArgument);
    }
}