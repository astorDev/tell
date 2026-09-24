using Superpower;

namespace Tell;

public record WorkingDirectory(string Path, string SearchPath, string? Change)
{
    public static string DefaultPath => Directory.GetCurrentDirectory();
    public static WorkingDirectory Default => new (DefaultPath, DefaultPath, null);

    public static bool TryUse(string workdirArgument, out WorkingDirectory workingDirectory)
    {
        var searchPath = System.IO.Path.Combine(DefaultPath, workdirArgument);
        var exists = Directory.Exists(searchPath);
        workingDirectory = new WorkingDirectory(
            Path: exists ? searchPath : DefaultPath,
            SearchPath: searchPath,
            Change: exists ? workdirArgument : null
        );

        return exists;
    }

    public MakefileParsingResult MakefileParsing(string? file)
    {
        var makefilePath = System.IO.Path.Combine(Path, file ?? "Makefile");
        if (!File.Exists(makefilePath))
        {
            throw new FileNotFoundException($"Makefile not found at {makefilePath}");
        }

        return MakefileParsingResult.FromPath(makefilePath);
    }
}

public record MakefileParsingResult(
    string Path,
    Makefile? Doc,
    Exception? ParsingException
)
{
    public Makefile DocOrThrowParsingError()
    {
        if (ParsingException is not null)
        {
            throw ParsingException;
        }
        
        if (Doc is not null)
        {
            return Doc;
        }

        throw new InvalidOperationException("MakefileParsingResult does not have a valid state: both Doc and ParsingException are null.");
    }

    public static MakefileParsingResult FromPath(string path)
    {
        var content = File.ReadAllText(path);
        try
        {
            var doc = Makefile.Parser.Parse(content);
            return new MakefileParsingResult(path, doc, null);
        }
        catch (Exception ex)
        {
            return new MakefileParsingResult(path, null, ex);
        }
    }

    public T Match<T>(Func<Makefile, T> onSuccess, Func<Exception, T> onParsingError)
    {
        if (Doc is not null)
        {
            return onSuccess(Doc);
        }
        if (ParsingException is not null)
        {
            return onParsingError(ParsingException);
        }
        throw new InvalidOperationException("MakefileParsingResult does not have a valid state: both Doc and ParsingException are null.");
    }
}