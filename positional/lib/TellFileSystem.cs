namespace Tell;

public record TellFileSystem(Folder WorkingDir, Copaster.File File)
{
    public static readonly SymbolsContainer Symbols = new(TellFilename.Option, WorkingDirectory.Argument);
    public record SymbolsContainer(Option<string> FilenameOption, Argument<Folder> ContextArgument);

    public static TellFileSystem From(ParseResult parsed)
    {
        var folder = parsed.GetRequiredValue(WorkingDirectory.Argument);
        var filename = parsed.GetRequiredValue(TellFilename.Option);
        var file = folder.File(filename);
        if (!file.Exists) throw new FileNotFoundException($"'{Path.GetFullPath(file.Path)}' does not exist.");

        return new TellFileSystem(folder, file);
    }
}

public record WorkingDirectory
{
    public static readonly Argument<Folder> Argument = new("workDir")
    {
        Description = "The working directory.",
        Arity = ArgumentArity.ExactlyOne,
        DefaultValueFactory = (result) => new("."),
        CustomParser = result =>
        {
            if (result.Tokens.Count == 0) return new(".");

            var token = result.Tokens[0].Value;
            var folder = new Folder(token);
            return folder.Exists ? folder : new(".");
        }
    };
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